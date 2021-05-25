import tushare as ts
from pandas import read_csv
import sqlite3


# 获取数据库连接
def getconn(conStr):
    try:
        conn = sqlite3.connect(conStr)
        return conn
    except Exception as e:
        raise e

# 执行sql语句，结果返回list
def executeAdapter(conn, sql):
    try:
        resultlist = []
        #conn = getconn()
        cursor = conn.cursor()
        cursor.execute(sql)
        for i in cursor:
            resultlist.append(list(i))
        cursor.close()
        conn.close()
        return resultlist
    except Exception as e:
        print(e)



if __name__ == '__main__':
    conn = getconn(r"E:\MyGit\BigDataFile\QSstrategy\HistoryData.db3")
    sql = "select symbol,close from daylevel order by tdate asc"
    resultlist = executeAdapter(conn, sql)
    historydic = dict()
    for item in resultlist:
        if item[0] in historydic.keys():
            historydic[item[0]].append(item[1])
        else:
            newlist = list()
            newlist.append(item[1])
            historydic[item[0]] = newlist
    #print(resultlist[0]['close'])
    print('finish')