#coding=utf-8
import sqlite3
 
class sqlite3Helper():
    def __init__(self,constr):
        self.conStr = constr

    # 获取数据库连接
    def getconn(self):
        try:
            conn = sqlite3.connect(self.conStr)
            return conn
        except Exception as e:
            raise e

    #执行sql，不返回结果（insert/update/delete）
    def executeNoQuery(self,sql):
        try:
            conn = self.getconn()
            cursor = conn.cursor()
            cursor.execute(sql)
            cursor.close()
            conn.commit()
            conn.close()
        except Exception as e:
            raise e

    #执行一个SQL数组，不返回结果（insert/update/delete）
    def executeListNoQuery(self,sqlList):
        try:
            conn = self.getconn()
            cursor = conn.cursor()
            for item in sqlList:
                cursor.execute(item)
            cursor.close()
            conn.commit()
            conn.close()
        except Exception as e:
            raise e

    
    def executeNoQueryparameter(self,sql,parameter):
        '''
        execute the sql that contains parameter
        :param sql: 
        :param parameter: the sql parameter dictionary
        :return: 
        '''
        try:
            conn = self.getconn()
            cursor = conn.cursor()
            cursor.execute(sql,parameter)
            conn.commit()
            cursor.close()
            conn.close()
        except Exception as e:
            raise e
    
    # 执行sql语句，结果返回list
    def executeAdapter(self,sql):
        try:
            resultlist = []
            conn = self.getconn()
            cursor = conn.cursor()
            cursor.execute(sql)
            for i in cursor:
                resultlist.append(list(i))
            cursor.close()
            conn.close()
            return resultlist
        except Exception as e:
            raise e
    
    # 批量插入,返回受影响行数
    # sql = "insert into daydata(date, exchange, symbol) values (?,?,?)"
    # values = [("11", "22","33"),    ("22", "33", "44")]
    def executeBulkCopy(self,sql,values):
        try:
            conn = self.getconn()
            resultlist = conn.executemany(sql, values)
            conn.commit()
            conn.close()
            return resultlist.rowcount
        except Exception as e:
            raise e

    # 关闭数据库连接
    def releaseconn(self):
        try:
            conn = self.getconn()
            conn.close()
        except Exception as e:
            raise e
            

#slatehelper = sqlite3Helper(r'E:\小程序\Python_Test_del\JQStockDataY.db')
#slatehelper = SqliteHelper.sqlite3Helper(r'E:\小程序\Python_Test_del\JQStockDataY.db')

#if __name__ == '__main__':
#    #r = slatehelper.executeAdapter('SELECT count(*) FROM daydata')
#    r = slatehelper.executeNoQuery('insert into daydata (date,exchange,symbol) values (1,2,3)')
#    #r = slatehelper.executeNoQuery('update daydata set open = 1 where open = 11')
#    print(r)