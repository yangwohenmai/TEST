
#coding=utf-8
# 导入requests库
import requests
# 导入文件操作库
import os
import re
import bs4
from bs4 import BeautifulSoup
import sys
#from util.mysql_DBUtils import mysql
import sqlite3
import time
"""
每次全量删除数据表，再全量插入新数据
"""
# 写入数据库
def write_db(param):
    try:
        #sql = "insert into house (url,housing_estate,position,square_metre,unit_price,total_price,follow,take_look,pub_date) "
        #sql = "(%(url)s,%(housing_estate)s, %(position)s,%(square_metre)s,"
        #sql = sql + "%(unit_price)s,%(total_price)s,%(follow)s,%(take_look)s,%(pub_date)s)"
        #sql = sql
        #mysql.insert(sql, param)
        cx = sqlite3.connect("E:/MyGit/TEST/GetAPIData/HouseMessage.db3")
        cu = cx.cursor()
        #cu.execute('''insert into keepalive values (2,'2019-04-18 16:59:26')''')
        #cu.execute('''CREATE TABLE stocks(date text,trans text,symbol text,gty real,price real)''')
        #cu.execute('''insert into stocks values('2016-01-05','BUY','RHAT',100,35.14)''')
        #cx.commit()

        #cu.execute('''create table catalog (id text ,url text ,housing_estate text ,position text ,square_metre text ,unit_price text ,total_price text ,follow text ,take_look text ,pub_date text )''')
        cu.execute('''insert into catalog values ('%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s' , '%s')'''%('1',param["url"],param["housing_estate"],param["position"],param["square_metre"],param["unit_price"],param["total_price"],param["follow"],param["take_look"],param["pub_date"],param["pagelink"],param["keyword"]))
        cx.commit()
        #c = cu.execute('''select * from catalog''')
        #print(list(c))


    except Exception as e:
        print(e)


# 删除数据
def del_db():
    try:

        cx = sqlite3.connect("E:/MyGit/TEST/GetAPIData/HouseMessage.db3")
        cu = cx.cursor()
        cu.execute('''delete from catalog''')
        cx.commit()
        #c = cu.execute('''select * from catalog''')
        #print(list(c))
        cx.close()
    except Exception as e:
        print(e)

#存储在当前IDE文件夹下 ， message：消息内容 ，  filmname：文件名
def WriteHere( message, filmname):
    strMessage = '\n' + time.strftime('%Y-%m-%d %H:%M:%S')
    strMessage += ':\n%s' % message

    fileName = os.path.join(os.getcwd(), filmname + '.txt')
    with open(fileName, 'a', encoding='utf-8') as f:
        f.write(strMessage)
# 主方法
def main():
    # 给请求指定一个请求头来模拟chrome浏览器
    headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36'}
    page_max = 100
    count = 0
    del_db()
    keyword = 'xuanqiao/'
    for i in range(1, int(page_max) + 1):
        if i == 1:
            house = 'https://qd.lianjia.com/ershoufang/shibei/'
            house = 'https://sh.lianjia.com/ershoufang/chuansha/'
            house = 'https://sh.lianjia.com/ershoufang/zhoupu/'
            house = 'https://sh.lianjia.com/ershoufang/' + keyword
        else:
            house = 'https://sh.lianjia.com/ershoufang/' + keyword + 'pg'+str(i)
        res = requests.get(house, headers=headers)
        soup = BeautifulSoup(res.text, 'html.parser')
        try:
            li_max = soup.find('ul', class_='sellListContent').find_all('li')
        except Exception as e:
            print(e)
            print(house)
            print(soup)
            continue

        for li in li_max:
            try:
                house_param = {}
                #  荣馨苑  | 3室2厅 | 115.91平米 | 南 北 | 毛坯 | 无电梯
                content = li.find('div', class_='houseInfo').text
                content = content.split("|")
                house_param['housing_estate'] = content[0]
                #house_param['square_metre'] = re.findall(r'-?\d+\.?\d*e?-?\d*?', content[2])[0]
                #上面的方法不好用了
                house_param['square_metre'] = content[1]
                # --------------------------------------------------------#
                #  位置 水清沟
                position = li.find('div', class_='positionInfo').find('a').text
                house_param['position'] = position
                # --------------------------------------------------------#
                totalprice = li.find('div', class_='totalPrice').text
                house_param['total_price'] = re.sub("\D", "", totalprice)
                unitprice = li.find('div', class_='unitPrice').text
                house_param['unit_price'] = re.sub("\D", "", unitprice)
                # --------------------------------------------------------#
                # 57人关注 / 共13次带看 / 6个月以前发布
                follow = li.find('div', class_='followInfo').text
                follow = follow.split("/")
                house_param['follow'] = re.sub("\D", "", follow[0])
                house_param['take_look'] = re.sub("\D", "", follow[1])
                # --------------------------------------------------------#
                # 二手房地址
                title_src = li.find('div', class_='title').find('a').attrs['href']
                house_param['url'] = re.sub("\D", "", title_src)
                res = requests.get(title_src, headers=headers)
                soup = BeautifulSoup(res.text, 'html.parser')
                # --------------------------------------------------------#
                # 挂牌时间(重要数据)
                pub_date = soup.find('div', class_='transaction').find_all('li')[0].find_all('span')[1].text
                # 发布日期
                house_param['pub_date'] = pub_date
                # 页面链接
                house_param['pagelink'] = house
                # 区域关键字
                house_param['keyword'] = keyword
                write_db(house_param)
                #WriteHere(house_param,'house')
                #没有数据库时，输出到console
                #print(house_param)
                count += 1
                print(count)
            except Exception as e:
                print(e)
        #mysql.end("commit")
    #mysql.dispose()


if __name__ == '__main__':
    main()