#coding=utf-8
#!/usr/bin/python
# 导入requests库
import requests
# 导入文件操作库
import os
import re
import bs4
from bs4 import BeautifulSoup
import sys
import json



# 主方法
def main():
    # 给请求指定一个请求头来模拟chrome浏览器
    headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36'}
    page_max = 100
    house = 'https://www.hkex.com.hk/?sc_lang=EN'
    res = requests.get(house, headers=headers)
    soup = BeautifulSoup(res.text, 'html.parser')
    #print(re.search('Base64-AES-Encrypted-Token',soup.text).span())
    #print (soup.text[2438:2465])
    #定位到Base64-AES-Encrypted-Token
    num = re.search('Base64-AES-Encrypted-Token',soup.text).span()
    print(num)
    print(num[1])
    #从定位点向后取120个字符
    numstr = soup.text[num[1]:num[1]+120]
    #print (re.search('return',numstr).span())
    #在120个字符中定位到return
    num1 = re.search('return',numstr).span()
    #从return的定位+2 向后取 100个字符
    numstr1 = numstr[num1[1]+2:num1[1]+100]
    print(numstr1)
    news = ''
    #对100个字符遍历，找到引号内的token
    for s in range(len(numstr1)):
        if numstr1[s] != '"':
            news = news+ numstr1[s]
            print(news)
        else:
            print('找到了token：'+news)
            result = 'https://www1.hkex.com.hk/hkexwidget/data/getequityquote?sym=1&token=%s&lang=eng&qid=NULL&callback=NULL' %news
            print(result)
            break

    res = requests.get(result, headers=headers)
    soup1 = BeautifulSoup(res.text, 'html.parser')
    
    print(json.loads(soup1.text[5:len(soup1.text)-1]))
    jsonstr = json.loads(soup1.text[5:len(soup1.text)-1])
    print("hi="+jsonstr['data']['quote']['hi'])
    print("db_updatetime="+jsonstr['data']['quote']['db_updatetime'])
    print("amt_os="+jsonstr['data']['quote']['amt_os'])
    print("ric="+jsonstr['data']['quote']['ric'])
    print("primaryexch="+jsonstr['data']['quote']['primaryexch'])
    #WriteTxt(json.loads(soup1.text[5:len(soup1.text)-1]), 'D:/', 'bbb1')

        
#存储在任意路径 ， message：消息内容 ， path：文件路径 ， filmname：文件名
def WriteTxt( message, path, filmname):
    strMessage = '\n' #+ time.strftime('%Y-%m-%d %H:%M:%S')
    strMessage += ':\n%s' % message
    fileName = os.path.join(path, "_" + filmname +  '.txt')
    with open(fileName, 'a', encoding='utf-8') as f:
        f.write(strMessage)



if __name__ == '__main__':
    main()