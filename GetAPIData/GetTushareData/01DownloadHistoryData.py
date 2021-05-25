import tushare as ts
from pandas import read_csv
import sqlite3
import time
import datetime as dt
import sys
'''
pro
'''
pro = ts.set_token('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
pro1 = ts.pro_api('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
df =[]
# 未复权

# 获取单个股票数据（未复权）
#df = pro.daily(ts_code='000001.SZ', start_date='20180701', end_date='20180718')
# 获取当天所有股票历史数据（未复权）
#df = pro.daily(trade_date='20180810')
# 获取多个股票数据（未复权）
#df = pro.daily(ts_code='000001.SZ,600000.SH',adj='hfq', start_date='20180701', end_date='20180718')
#获取所有股票列表
#df = pro1.stock_basic(exchange='', list_status='L', fields='ts_code,symbol,name,area,industry,list_date')
# 获取复权数据（通用接口）
#df = ts.pro_bar(ts_code='000001.SZ', start_date='20210401', end_date='20210517', ma=[5, 20, 50])
#df = ts.pro_bar(ts_code='000001.SZ', adj='qfq', start_date='20210401', end_date='20210517')
'''
tushare
'''
# 获取个股历史交易数据,一次性获取近3年全部日k线数据
#df = ts.get_hist_data('000001')
#df = ts.get_hist_data('000001',start='2015-01-05',end='2021-01-09')
#获取大盘指数实时行情列表
#df = ts.get_index()
# 获取周k线数据
#df = ts.get_hist_data('600848', ktype='W') 
# 一次性获取当前交易所有股票的行情数据（实时行情）
#df = ts.get_today_all()
#df = df[['code','nmc']]
# 获取实时分笔数据
#df = ts.get_realtime_quotes('000581') #Single stock symbol
#df = df[['code','name','price','bid','ask','volume','amount','time']]
print(df)

# 创建一个txt文件，文件名为mytxtfile,并向文件写入msg
def text_create(filename, msg, path=''):
    full_path = path + filename + '.csv'  # 也可以创建一个.doc的word文档
    file = open(full_path, 'a+')
    file.write(msg)   #msg也就是下面的Hello world!
    # file.close()

# 保存流通市值到csv
def SaveStockNmc():
    df = ts.get_today_all()
    df = df[['code','nmc']]
    for col in df.values:
        msg = str(col[0]) + ',' + str(col[1]*10000) + '\n'
        text_create('test',msg,r'E:/MyGit/SomethingTemp/清水策略/')
    print('nmc has save to csv')

# 加载流通市值文件
def GetStockNmcDic():
    dataframe = read_csv(r'E:/MyGit/SomethingTemp/清水策略/test.csv',dtype='str', header=0)
    stockdic = dict()
    for i in dataframe.values.tolist():
        stockdic[i[0]] = float(i[1]) * 0.01
    return stockdic

# 保存股票列表到csv
def SaveAllStockList():
    pro = ts.pro_api('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
    df = pro.stock_basic(exchange='', list_status='L', fields='ts_code,symbol')
    print(df)
    for col in df.values:
        msg = str(col[0]) + ',' + str(col[1]) + '\n'
        text_create('stocklist',msg,r'E:/MyGit/SomethingTemp/清水策略/')
    print('AllStockList has save to csv')

# 加载股票列表
def GetAllStockListDic():
    dataframe = read_csv(r'E:/MyGit/SomethingTemp/清水策略/stocklist.csv',dtype='str', header=0)
    allstocklistdic = dict()
    for i in dataframe.values.tolist():
        allstocklistdic[i[0]] = i[1]
    return allstocklistdic




# 获取完整股票列表
def SaveAllStockList():
    pro = ts.pro_api('6d9ac99d25b0157dcbb1ee3d35ef1250e5295ff80bb59741e1a56b35')
    df = pro.stock_basic(exchange='', list_status='L', fields='ts_code,symbol')
    allstocklistdic = dict()
    for col in df.values:
        allstocklistdic[col[0]] = col[1]
    print('AllStockList has save')
    return allstocklistdic


# 写入数据库
def write_db(param):
    try:
        cx = sqlite3.connect("E:\MyGit\SomethingTemp\清水策略\HistoryData.db3")
        cu = cx.cursor()
        cu.execute('''insert into DayLevel (symbol, tdate, open, high, low, close, lclose, vol, amount) values ('%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' )'''\
            %(param["symbol"],param["tdate"],param["open"],param["high"],param["low"],param["close"],param["lclose"],param["vol"],param["amount"]))
        cx.commit()
    except Exception as e:
        print(e)

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
        #c = cu.execute('''select * from catalog''')
        #print(list(c))
        conn.close()
    except Exception as e:
        print(e)

#SaveStockNmc()
#GetStockNmcDic()

def SaveDataToSqlite(constr, stocklistdic, begindate, enddate):
    try:
        count = 0
        sqlstrlist = list()
        for code in stocklistdic.keys():
            df = ts.pro_bar(ts_code=code, start_date=begindate, end_date=enddate)
            count += 1
            #print(df)
            #print(df.values)
            if len(df.values) < 6:
                continue
            for i in range(len(df.values)):
                #print(df.values[i])
                value = df.values[i]
                param = {'symbol':value[0], 'tdate':value[1], 'open':value[2], 'high':value[3], \
                    'low':value[4], 'close':value[5], 'lclose':value[6], 'vol':value[9], 'amount':value[10] }
                sqlstrlist.append('''insert into DayLevel (symbol, tdate, open, high, low, close, lclose, vol, amount) values ('%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' );'''\
                %(param["symbol"],param["tdate"],param["open"],param["high"],param["low"],param["close"],param["lclose"],param["vol"],param["amount"]))
            finishlist.append(code)
            if count % 100 == 0:
                #print('begin insert: ',dt.datetime.now())
                writeSqlList_db(constr, sqlstrlist)
                #print('finish insert: ',dt.datetime.now())
                print(count, ' finish', dt.datetime.now())
                sqlstrlist = list()
            if count % 500 == 0:
                print('因为贫穷导致的休眠1min')
                time.sleep(30)
            #print(df['open'].tolist())
        if len(sqlstrlist) > 0:
            writeSqlList_db(constr, sqlstrlist)
        print('finish all')
    except Exception as e:
        print(e)
        print(finishlist)


if __name__ == '__main__':
    inputdate = ("input begindate and enddate, use ',' to split date, such as: '20210512,20210519'")
    inputdatelist = inputdate.split(",")
    stocklistdic = SaveAllStockList()
    finishlist = list()
    constr = "E:\MyGit\SomethingTemp\清水策略\HistoryData.db3"
    # 每次更新前，先删除历史数据
    del_db(constr)
    # 删除后在重新下载最近6天的历史数据
    SaveDataToSqlite(constr, stocklistdic, inputdatelist[0], inputdatelist[1])
    print('finish')
    sys.exit(0)
    
    
    
    try:
        for code in stocklistdic.keys():
            df = ts.pro_bar(ts_code=code, start_date='20210518', end_date='20210518')
            #print(df)
            #print(df.values)
            for i in range(len(df.values)):
                #print(df.values[i])
                value = df.values[i]
                paramdic = {'symbol':value[0], 'tdate':value[1], 'open':value[2], 'high':value[3], \
                    'low':value[4], 'close':value[5], 'lclose':value[6], 'vol':value[9], 'amount':value[10] }
                #print(paramdic)
                write_db(paramdic)
            print(code, ' finish')
            finishlist.append(code)
            #print(df['open'].tolist())
        print(finishall)
    except Exception as e:
        print(e)
        print(finishlist)
    #SaveAllStockList()
    #GetAllStockListDic()