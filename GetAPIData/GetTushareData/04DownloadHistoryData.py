import tushare as ts
from pandas import read_csv
import sqlite3
import time
import datetime as dt
import sys

pro = ts.set_token('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
pro1 = ts.pro_api('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')

# 获取完整股票列表
def SaveAllStockList():
    pro = ts.pro_api('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
    df = pro.stock_basic(exchange='', list_status='L', fields='ts_code,symbol')
    allstocklistdic = dict()
    for col in df.values:
        allstocklistdic[col[0]] = col[1]
    print('AllStockList has save')
    return allstocklistdic

# 批量写入多条数据库
def writeSqlList_db(constr, sqllist):
    try:
        conn = getconn(constr)
        cursor = conn.cursor()
        for sql in sqllist:
            cursor.execute(sql)
        conn.commit()
        conn.close()
    except Exception as e:
        print(e)

# 获取数据库连接
def getconn(conStr):
    try:
        conn = sqlite3.connect(conStr)
        return conn
    except Exception as e:
        raise e

# 删除数据
def del_db(constr):
    try:
        conn = getconn(constr)
        cursor = conn.cursor()
        cursor.execute('''delete from daylevel''')
        cursor.close()
        conn.commit()
        conn.close()
    except Exception as e:
        print(e)

def SaveDataToSqlite(constr, stocklistdic, begindate, enddate):
    try:
        count = 0
        sqlstrlist = list()
        finishlist = list()
        for code in stocklistdic.keys():
            df = ts.pro_bar(ts_code=code, start_date=begindate, end_date=enddate)
            count += 1
            if len(df.values) < 6:
                continue
            for i in range(len(df.values)):
                value = df.values[i]
                param = {'symbol':value[0], 'tdate':value[1], 'open':value[2], 'high':value[3], \
                    'low':value[4], 'close':value[5], 'lclose':value[6], 'vol':value[9], 'amount':value[10] }
                sqlstrlist.append('''insert into DayLevel (symbol, tdate, open, high, low, close, lclose, vol, amount) values ('%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' );'''\
                %(param["symbol"],param["tdate"],param["open"],param["high"],param["low"],param["close"],param["lclose"],param["vol"],param["amount"]))
            finishlist.append(code)
            if count % 100 == 0:
                writeSqlList_db(constr, sqlstrlist)
                print(count, ' finish', dt.datetime.now())
                sqlstrlist = list()
            if count % 500 == 0:
                print('因为贫穷导致的休眠1min')
                time.sleep(30)
        if len(sqlstrlist) > 0:
            writeSqlList_db(constr, sqlstrlist)
            print(count, ' finish', dt.datetime.now())
        print('finish all')
    except Exception as e:
        print(e)
        print(finishlist)


if __name__ == '__main__':
    #print("input begindate and enddate, use ',' to split date, such as: '20210512,20210519'")
    #inputdate = input()
    #inputdatelist = inputdate.split(",")
    #startdate = inputdatelist[0]
    #enddate = inputdatelist[1]    
    stocklistdic = SaveAllStockList()
    startdate = dt.datetime.today() - dt.timedelta(8)
    enddate = dt.datetime.today() - dt.timedelta(1)
    constr = "E:\MyGit\BigDataFile\QSstrategy\HistoryData.db3"
    # 每次更新前，先删除历史数据
    del_db(constr)
    # 删除后在重新下载最近6天的历史数据
    SaveDataToSqlite(constr, stocklistdic, startdate.strftime('%Y%m%d'), enddate.strftime('%Y%m%d'))
    print('finish')
    #sys.exit(0)
    
    
    