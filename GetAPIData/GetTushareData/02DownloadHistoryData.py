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


# 创建一个txt文件，文件名为mytxtfile,并向文件写入msg
def text_create(filename, msg, path=''):
    full_path = path + filename + '.csv'  # 也可以创建一个.doc的word文档
    file = open(full_path, 'a+')
    file.write(msg)   #msg也就是下面的Hello world!
    # file.close()

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
    print("input begindate and enddate, use ',' to split date, such as: '20210512,20210519'")
    inputdate = input()
    inputdatelist = inputdate.split(",")
    stocklistdic = SaveAllStockList()
    finishlist = list()
    constr = "E:\MyGit\SomethingTemp\清水策略\HistoryData.db3"
    # 每次更新前，先删除历史数据
    del_db(constr)
    # 删除后在重新下载最近6天的历史数据
    SaveDataToSqlite(constr, stocklistdic, inputdatelist[0], inputdatelist[1])
    print('finish')
    #sys.exit(0)
    
    
    