
#coding:utf-8
from threading import Timer
import time
import os
import csv
import tushare as ts
import numpy as np
import pandas as pd
import datetime
from jqdatasdk import *
from pathlib import Path

#前置全局变量
global today, todaytime, yesterday, yesterdaytime, tommow, tommowtime, pandastime, cwf, alltradeday, homefolder
#保存当前日期
pandastime = pd.Timestamp("2017-6-19 9:13:45")
today = datetime.date.today()
todaytime = datetime.datetime.strptime(str(today),'%Y-%m-%d')
tommow = today + datetime.timedelta(days=1)
tommowtime = datetime.datetime.strptime(str(tommow),'%Y-%m-%d')
yesterday = today - datetime.timedelta(days=1)
yesterdaytime = datetime.datetime.strptime(str(yesterday),'%Y-%m-%d')
#建立聚宽数据的最早日期线
jqmonthheadtime = datetime.datetime(2005, 2, 1, 0 ,0)
jqweekheadtime = datetime.datetime(2005, 1, 15, 0 ,0)
jqdayheadtime = datetime.datetime(2005, 1, 5, 0 ,0)
jq60mheadtime = datetime.datetime(2005, 1, 4, 10 ,31)
jq30mheadtime = datetime.datetime(2005, 1, 4, 10 ,1)
jq15mheadtime = datetime.datetime(2005, 1, 4, 9 ,46)
jq5mheadtime = datetime.datetime(2005, 1, 4, 9 ,36)
jq1mheadtime = datetime.datetime(2005, 1, 4, 9 ,31)
cwf = os.getcwd()
alltradeday = get_all_trade_days()
homefolder = os.path.dirname(os.path.abspath(__file__))

#公共函数与模块
#读取config获取所有帐号并登录
def getconfig():
    global jquser, jqpw
    print('目前共有帐号1,2,3,4,5,6,7,8,9,10')
    x = input('请输入你想要使用的帐号序号:')
    x = int(x)
    jquser, jqpw = readconfig(x)
    auth(jquser, jqpw)
def querylast():
    tmp = get_query_count()
    count, total = tmp['spare'], tmp['total']
    return count, total
#获取全部股票列表并存档
def savestklist():
    #全局变量：ts所有股票信息，jq所有股票信息，jqA50指数，jq上证指数，jq深证成指，jq沪深300，jq创业板指，jq中小板指，jq上证股票列表，jq深证股票列表
    global jqallstockdata, jqsz50code, jqszzscode, jqszczcode, jqhs300code, jqcybzcode, jqzxbzcode, jqcodelistszzs, jqcodelistszcz
    #获取所有本地股票信息
    jqallstockdata = pd.read_csv('./stockdb/jqallstock.csv', encoding='GBK', index_col=0)
    #获取所有本地指数信息
    jqallindex = pd.read_csv('./stockdb/jqallindex.csv', encoding='GBK', index_col=0)
    return

def readconfig(x):
    config = open('./jqconfig.cfg', mode='r', buffering=-1, encoding='utf-8', errors=None, newline=None, closefd=True, opener=None)
    tmp = config.readlines()
    jquser = tmp[x].split(',', 1)[0].strip()
    jqpw = tmp[x].split(',', 1)[-1].strip()
    config.close()
    return jquser, jqpw

def readlocallist():
    global jqlist, donelist
    donelist = []
    tmp = open('./stockdb/jqlist.cfg', mode='r', buffering=-1, encoding=None, errors=None, newline=None, closefd=True, opener=None)
    jqlist = tmp.readlines()
    tmp.close()
    return

def writelocallist():
    tmp = open('./stockdb/jqlist.cfg', mode='w+', buffering=-1, encoding=None, errors=None, newline=None, closefd=True, opener=None)
    tmp.writelines(jqlist)
    tmp.close()
    return

def getdatajqnew(jqcode, count, startdate, jqheadtime):
    tmp = locals()
    enddatetime = tommowtime
    datacount = 0
    if enddatetime > startdate and enddatetime > jqheadtime:
        enddt = enddatetime
        print('----++首次拉取周期%s的数据++----' % (count))
        jqdata = get_bars(jqcode, 999999, unit=count, fields=['date', 'open', 'close', 'high', 'low', 'volume', 'money'], include_now=False, end_dt=enddt, fq_ref_date=None)
        time.sleep(1)
        loadingcount = 0
        while len(jqdata['close'].tolist()) == 0 and loadingcount < 5:
            print('--拉取股票%s的数据为空--' % (jqcode))
            jqdata = get_bars(jqcode, 999999, unit=count, fields=['date', 'open', 'close', 'high', 'low', 'volume', 'money'], include_now=False, end_dt=enddt, fq_ref_date=None)
            time.sleep(1)
            loadingcount = loadingcount + 1
            print('--尝试重新获取数据第%s次--' % (loadingcount))
        else:
            datacount = datacount + 1
            tmp['jqpandas' + str(datacount)] = jqdata
            tmptime = jqdata['date'].tolist()[0]
            if type(tmptime) == type(today):
                tmptime = datetime.datetime.strptime(str(tmptime),'%Y-%m-%d')
            elif type(tmptime) == type(pandastime):
                tmptime = tmptime.to_pydatetime()
            enddatetime = tmptime
        jqenddate = tmp['jqpandas' + str(datacount)].date.tolist()[0]
    finaldata = tmp['jqpandas' + str(datacount)]
    datacount = datacount - 1
    while datacount > 0:
        finaldata = finaldata.append(tmp['jqpandas' + str(datacount)])
        datacount = datacount - 1
    return finaldata

def getdata(jqcode, n, remain):
    jqcode = jqcode.strip('\n')
    count, total = querylast()
    #获取当前股票的上市日期:聚宽数据，数据格式datetime.date
    starttimestr = jqallstockdata.loc[jqcode, 'start_date']
    starttime = datetime.datetime.strptime(starttimestr,'%Y-%m-%d')
    #提取六位代码（tushare使用的代码后缀需要拼接）
    code = jqcode.split('.', 1)[0].strip()
    tscodeadd = '.SH' if jqcode.split('.', 1)[1].strip() == 'XSHG' else '.SZ'
    tscode = code + tscodeadd
    #确定代码的市场归属
    market = 'shanghai' if jqcode.split('.', 1)[1].strip() == 'XSHG' else 'shenzhen'
    jqdir = Path(str('./stockdb/joinquant/' + market + '/' + code))
    tsdir = Path(str('./stockdb/tushare/' + market + '/' + code))
    #确定数据路径
    dbdirjqmonth = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'month.csv'))
    dbdirtsmonth = Path(str('./stockdb/tushares/' + market + '/' + code + '/' + code + 'month.csv'))
    dbdirjqweek = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'w.csv'))
    dbdirtsweek = Path(str('./tsstockdb/tushare/' + market + '/' + code + '/' + code + 'w.csv'))
    dbdirjqday = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'd.csv'))
    dbdirtsday = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'd.csv'))
    dbdirjq60m = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'm60.csv'))
    dbdirts60m = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'm60.csv'))
    dbdirjq30m = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'm30.csv'))
    dbdirts30m = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'm30.csv'))
    dbdirjq15m = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'm15.csv'))
    dbdirts15m = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'm15.csv'))
    dbdirjq5m = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'm5.csv'))
    dbdirts5m = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'm5.csv'))
    dbdirjq1m = Path(str('./stockdb/joinquant/' + market + '/' + code + '/' + code + 'm1.csv'))
    dbdirts1m = Path(str('./stockdb/tushare/' + market + '/' + code + '/' + code + 'm1.csv'))
    #tsdata = pro.monthly(tscode, fields='ts_code,trade_date,open,high,low,close,pre_close,change,pct_chg,vol,amount')
    if count > remain:
        if jqdir.is_dir():
            pass
        else:
            print('文件夹不存在，新建文件夹')
            os.makedirs(jqdir)
        print('开始获取数据')
        if n == 'a':
            print('每只股票完整数据最多可能会消耗125万次聚宽次数')
            datajq = getdatajqnew(jqcode, '1M', starttime, jqmonthheadtime)
            datajq.to_csv(dbdirjqmonth, encoding='GBK')
            datajq = getdatajqnew(jqcode, '1w', starttime, jqweekheadtime)
            datajq.to_csv(dbdirjqweek, encoding='GBK')
            datajq = getdatajqnew(jqcode, '1d', starttime, jqdayheadtime)
            datajq.to_csv(dbdirjqday, encoding='GBK')
            datajq = getdatajqnew(jqcode, '60m', starttime, jq60mheadtime)
            datajq.to_csv(dbdirjq60m, encoding='GBK')
            datajq = getdatajqnew(jqcode, '30m', starttime, jq30mheadtime)
            datajq.to_csv(dbdirjq30m, encoding='GBK')
            datajq = getdatajqnew(jqcode, '15m', starttime, jq15mheadtime)
            datajq.to_csv(dbdirjq15m, encoding='GBK')
            datajq = getdatajqnew(jqcode, '5m', starttime, jq5mheadtime)
            datajq.to_csv(dbdirjq5m, encoding='GBK')
            datajq = getdatajqnew(jqcode, '1m', starttime, jq1mheadtime)
            datajq.to_csv(dbdirjq1m, encoding='GBK')
        elif n == 'month':
            print('+++++++++获取月线数据+++++++++++')
            datajq = getdatajqnew(jqcode, '1M', starttime, jqmonthheadtime)
            datajq.to_csv(dbdirjqmonth, encoding='GBK')
        elif n == 'week' or n == 'a':
            print('+++++++++获取周线数据+++++++++++')
            datajq = getdatajqnew(jqcode, '1w', starttime, jqweekheadtime)
            datajq.to_csv(dbdirjqweek, encoding='GBK')
        elif n == 'day' or n == 'a':
            print('+++++++++获取日线数据+++++++++++')
            datajq = getdatajqnew(jqcode, '1d', starttime, jqdayheadtime)
            datajq.to_csv(dbdirjqday, encoding='GBK')
        elif n == '60m' or n == 'a':
            print('+++++++++获取60分钟线数据+++++++++')
            datajq = getdatajqnew(jqcode, '60m', starttime, jq60mheadtime)
            datajq.to_csv(dbdirjq60m, encoding='GBK')
        elif n == '30m' or n == 'a':
            print('+++++++++获取30分钟线数据+++++++++')
            datajq = getdatajqnew(jqcode, '30m', starttime, jq30mheadtime)
            datajq.to_csv(dbdirjq30m, encoding='GBK')
        elif n == '15m' or n == 'a':
            print('+++++++++获取15分钟线数据+++++++++')
            datajq = getdatajqnew(jqcode, '15m', starttime, jq15mheadtime)
            datajq.to_csv(dbdirjq15m, encoding='GBK')
        elif n == '5m' or n == 'a':
            print('+++++++++获取5分钟线数据+++++++++')
            datajq = getdatajqnew(jqcode, '5m', starttime, jq5mheadtime)
            datajq.to_csv(dbdirjq5m, encoding='GBK')
        elif n == '1m' or n == 'a':
            print('+++++++++获取1分钟线数据+++++++++')
            datajq = getdatajqnew(jqcode, '1m', starttime, jq1mheadtime)
            datajq.to_csv(dbdirjq1m, encoding='GBK')
        x = 1
        return x
    else:
        print('帐号剩余的聚宽数据获取次数不足')
        x = 0
        return x

def followorder():
    count, total = querylast()
    print('|+++++++++当前帐号聚宽数据获取余额为%s次,上限为%s次++++++++++' % (count, total))
    print('---当前文档内第一个代码为%s---' % (jqlist[0]))
    print('|++++++++++++请选择你需要进行的操作，菜单说明如下++++++++++++++++]')
    print('|自动下载列表中股票的完整历史数据直到剩余次数少于1250000请输入:1')
    print('|             手动选择周期下载文档中首只股票的分时数据请输入:2')
    print('|                   下载任意输入的股票的单个分时数据请输入:3')
    print('|                       查看今日剩余的聚宽调用次数请输入:4')
    print('|                     退出当前聚宽帐号并切换帐号请输入:5')
    print('|                   从文档中删除第一个股票代码请输入:d')
    print('|                   获取所有的股票交易日并保存请输入:td')
    print('|+++++++++++结束程序请留空或输入其他任意字符++++++++++++')
    order = input('=============请输入你想要进行的操作=============:')
    if order == '1':
        while count > 1250000:
            print('--当前文档内第一个代码为%s--' % (jqlist[0]))
            print('++++即将开始获取完整数据的股票为:%s' % (jqlist[0]))
            x = getdata(jqlist[0], 'a', 1250000)
            if x == 1:
                print('[   股票%s完整数据获取似乎顺利完成了   ]' % (jqlist[0]))
                del jqlist[0]
                writelocallist()
                print('_____完成的股票代码已从本地列表中删除_____')
                count, total = querylast()
            else:
                pass
        else:
            print('!!!!!!!!聚宽剩余数据获取次数不足!!!!!!!!')
            followorder()
            exit()
            return
    elif order == '2':
        print('完整股票数据下载顺序为月，周，日，60分，30分，15分，5分，1分')
        print('---当前文档内第一个代码为%s---' % (jqlist[0]))
        jqcode = jqlist[0]
        print('输入说明:月线为month，周线为week，日线为day，60分线为60m，30分线为30m，15分线为15m，5分线为5m，1分线为1m，注意小写')
        cycle = input('请输入你想要手工下载数据的BAR周期:')
        x = getdata(jqcode, cycle, 0)
        if x == 1:
            print('--股票%s指定数据获取似乎顺利完成了--' % (jqcode))
            followorder()
            exit()
        else:
            print('聚宽剩余数据获取次数不足')
            followorder()
            exit()
            return
    elif order == '3':
        print('完整股票数据下载顺序为月，周，日，60分，30分，15分，5分，1分')
        print('输入说明:沪市股票代码结尾请补上.XSHG，深市股票代码结尾请补上.XSHE，注意大写')
        jqcode = input('请输入你想要手工下载数据的股票代码:')
        print('输入说明:月线为month，周线为week，日线为day，60分线为60m，30分线为30m，15分线为15m，5分线为5m，1分线为1m，注意小写')
        cycle = input('请输入你想要手工下载数据的BAR周期:')
        x = getdata(jqcode, cycle, 0)
        if x == 1:
            print('--股票%s指定数据获取似乎顺利完成了--' % (jqcode))
            followorder()
            exit()
        else:
            print('聚宽剩余数据获取次数不足')
            followorder()
            exit()
            return
    elif order == '4':
        count, total = querylast()
        print('--当前帐号聚宽数据获取上限为%s，剩余获取次数为%s--' % (total, count))
        followorder()
        exit()
        return
    elif order == '5':
        print('目前共有帐号1')
        x = input('请输入你想要使用的帐号序号:')
        x = int(x)
        jquser, jqpw = readconfig(x)
        logout()
        auth(jquser, jqpw)
        followorder()
        exit()
        return
    elif order == 'd':
        del jqlist[0]
        writelocallist()
        print('文档中的首个股票代码已从本地列表中删除')
        followorder()
        exit()
        return
    elif order == 'td':
        day = get_all_trade_days()
        daypd = pd.Series(day[::-1])
        print(type(daypd))
        daypd.to_csv(Path(str(homefolder + '/stockdb/tradedate.csv')), encoding='GBK', index=False)
        print('所有交易日已保存')
        followorder()
        exit()
        return
    else:
        exit()
        return

#执行部分
#读取config获取所有帐号并登录
getconfig()

#获取全部股票列表并存档
savestklist()

#从本地历史文件读取需要操作的股票列票
readlocallist()

#文字交互界面
followorder()

#检查文件夹
#for code in jqcodelistszzs:
    #checkfolder(code)
#for code in jqcodelistszcz:
    #checkfolder(code)

#更新全部股票行情
#for code in jqcodelistszzs:
    #updatastockdbjq(code)
#for code in jqcodelistszcz:
    #updatastockdbjq(code)