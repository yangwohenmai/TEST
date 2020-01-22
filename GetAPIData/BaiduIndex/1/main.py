from get_index import BaiduIndex
import requests
from config import COOKIES, PROVINCE_CODE, CITY_CODE

if __name__ == "__main__":
    #测试
    #url = "http://i.baidu.com"
    #wcookie = {"BDUSS":COOKIES}
    #HTML = requests.get(url,cookies=wcookie).content
    #print(HTML)

    #正式
    keywords = ['猪肉']
    baidu_index = BaiduIndex(keywords, '2019-08-01', '2020-01-21')
    for index in baidu_index.get_index():
        print(index)