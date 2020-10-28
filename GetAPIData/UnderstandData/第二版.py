import requests
from bs4 import BeautifulSoup
import os
import time;
import random
 
#pip install BeautifulSoup4 -i  https://pypi.douban.com/simple
#pip install requests -i  https://pypi.douban.com/simple
 
# http请求头
Hostreferer = {
    'Referer': 'http://www.mzitu.com',
     
    'Upgrade-Insecure-Requests': '1',
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36'
}
 
# 此请求头Referer破解盗图链接
Picreferer = {
    # 'User-Agent': 'Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)',
    # 'User-Agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3679.0 Safari/537.36',
    # 'Referer': 'http://i.meizitu.net',
    # https://www.mzitu.com/224497/3
    'Referer': 'http://www.mzitu.com',
    'Upgrade-Insecure-Requests': '1',
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36'
}
all_url = 'https://www.mzitu.com'
# 对mzitu主页all_url发起请求，将返回的HTML数据保存，便于解析
start_html = requests.get(all_url, headers=Hostreferer)
  
soup = BeautifulSoup(start_html.text, "html.parser") # 缩进格式
page = soup.find_all('a', class_='page-numbers')
# 最大页数
max_page = page[-2].text
for n in range(1, int(max_page) + 1):
    path = 'mzitu/' #存储路径，不要带特殊符号
    all_url = 'https://www.mzitu.com' #重新赋值
    if n!=1:
        all_url=  all_url+"/page/"+str(n)+"/"
    print('开始爬第 %s 页, 网址是 %s' % (n , all_url))
    start_html = requests.get(all_url, headers=Hostreferer)
    soup = BeautifulSoup(start_html.text, "html.parser")
#   alt =  soup.find(id='pins').find_all('a', target='_blank').find_all('img',class_='lazy').get('alt')
    hrefs = soup.find(id='pins').find_all('a', target='_blank'); #根据ID找
 
    for href in hrefs:
        imgs = href.find('img',class_='lazy')
        if imgs == None:
            break
        alt = imgs.get('alt')
        url = href.get('href')
        start_html2 = requests.get(url, headers=Hostreferer)
        soup2 = BeautifulSoup(start_html2.text, "html.parser")  # 缩进格式
        page2 = soup2.find('div', class_='pagenavi').find_all('a')
        # print (page2[0])
        max_page2 = page2[-2].text
        path = path + alt.strip().replace('?', '')
        path = path.replace(':', '')
        path = path.replace('|', '')
        path = path.replace('*', '')
        path = path.replace('<', '')
        path = path.replace('>', '')
        if (os.path.exists(path)):
            pass
            # print('目录已存在')
        else:
            os.makedirs(path)
        for m in range(1,int(max_page2)):
 
            time.sleep(random.randint(1,5))
            # alt = href.find('img', class_='lazy').get('alt');
            # url = href.get('href');
            url3 = url+'/'+str(m)+'/'
            print('开始爬→%s' % url3)
            start_html3 = requests.get(url3, headers=Hostreferer)
            soup3 = BeautifulSoup(start_html3.text, "html.parser")  # 缩进格式
            picSrc = soup3.find('div', class_='main-image').find('a').find('img').get('src');#.get('src');#.get('src'); #div class="main-image"
            # imglist = #获取当前页上所有的子连接, 不包含class="box"
            html = requests.get(picSrc, headers=Picreferer)
 
            # 提取图片名字
            file_name = path+'/'+picSrc.split(r'/')[-1]
            # 保存图片
            f = open(file_name, 'wb')
            f.write(html.content)
            f.close()
            print('图片保存到%s' % file_name)