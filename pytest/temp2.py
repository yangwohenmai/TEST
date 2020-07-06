
import requests
import json
import urllib.request
import pandas as pd
import time
 
class TrendsSpider:
    def __init__(self):
        self.keywork = ''
        self.token_url = 'https://trends.google.com/trends/api/explore?'
        self.token_headers = {
            "accept": "application/json, text/plain, */*",
            "accept-encoding": "gzip, deflate, br",
            "accept-language": "zh-CN,zh;q=0.9,en;q=0.8",
            "cookie": "__utmz=10102256.1547535267.1.1.utmcsr=baidu.com|utmccn=(referral)|utmcmd=referral|utmcct=/link; __utma=10102256.1208088738.1547535267.1547603196.1547605753.5; __utmc=10102256; 1P_JAR=2019-1-16-2; NID=154=HRqReD8V9OqXpwqDWsXP2konOU45Qu7UNi8KFhOJdgcU6EJezu3FoXe526_uxOvvnXPVXMOlc5QS9eerqT3oLA_Uug0xwjPeSRn8d93wwTmmNDpTCcGgh1R2aiieUAREn1lO5HirzI2RMI5H7Tt2eA04PKI4RuzCqRSAbO44zgg",
            "referer": "https://trends.google.com/trends/explore?date=now%207-d&geo=US&q={}".format(self.keywork),
            "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
            "x-client-data": "CJe2yQEIprbJAQjBtskBCKmdygEIqKPKAQi/p8oBCOynygEI46jKARj5pcoB",
        }
        self.token_req = {}
        self.token_value = {}
        # ----------分隔线---------------
        self.data_url = 'https://trends.google.com/trends/api/widgetdata/multiline?'
        self.data_headers = {
            "accept": "application/json, text/plain, */*",
            "accept-encoding": "gzip, deflate, br",
            "accept-language": "zh-CN,zh;q=0.9,en;q=0.8",
            "cookie": "__utmz=10102256.1547535267.1.1.utmcsr=baidu.com|utmccn=(referral)|utmcmd=referral|utmcct=/link; __utmc=10102256; __utma=10102256.1208088738.1547535267.1547605753.1547610772.6; __utmt=1; __utmb=10102256.1.10.1547610772; NID=154=HRqReD8V9OqXpwqDWsXP2konOU45Qu7UNi8KFhOJdgcU6EJezu3FoXe526_uxOvvnXPVXMOlc5QS9eerqT3oLA_Uug0xwjPeSRn8d93wwTmmNDpTCcGgh1R2aiieUAREn1lO5HirzI2RMI5H7Tt2eA04PKI4RuzCqRSAbO44zgg; 1P_JAR=2019-1-16-3",
            "referer": "https://trends.google.com/trends/explore?date=now%207-d&geo=US&q={}".format(self.keywork),
            "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
            "x-client-data": "CJe2yQEIprbJAQjBtskBCKmdygEIqKPKAQi/p8oBCOynygEI46jKARj5pcoB",
        }
        self.data_req = {}
        self.data_value = {}
        self.proxies = {}
        self.temp_data = []
        self.sava_data = []
 
    def get_token(self):
        '''获取token需要的参数'''
        # 1.参数req
        self.token_req['comparisonItem'] = [
            {"keyword": urllib.request.quote(self.keywork).replace(' ', '+'), "geo": "US", "time": "now 7-d"}
        ]
        self.token_req['category'] = 0
        self.token_req['property'] = ''
        # 2.参数value
        self.token_value['hl'] = 'zh-CN'
        self.token_value['tz'] = '-480'
        self.token_value['req'] = urllib.request.quote(str(self.token_req))
        for index in self.token_value:
            self.token_url = self.token_url + index + '=' + self.token_value[index] + '&'
        print("token_拼接后url1", self.token_url)
        # 后面两个参数很重要：忽略ssl证书和拒绝默认的重定
        time.sleep(1)
        # self.proxies=
        results = requests.get(url=self.token_url, headers=self.token_headers, verify=False, allow_redirects=False)
        page = results.content.decode("utf-8")
        data = json.loads(page[5:])
        token = data['widgets'][0]['token']
        token_time = data['widgets'][0]['request']["time"]
        print("token_time为：", token_time)
        print("获取到token为：", token)
        return token, token_time
 
    def get_google_trend(self):
        '''获取trend的指数'''
        token, time_temp = self.get_token()
        self.data_req['time'] = time_temp  # 之前不知道什么鬼
        self.data_req['resolution'] = "HOUR"  # 小时
        self.data_req['locale'] = "zh-CN"
        self.data_req['comparisonItem'] = [{"geo": {"country": "US"},
                                            "complexKeywordsRestriction": {
                                                "keyword": [
                                                    {"type": "BROAD",
                                                     "value": self.keywork}
                                                ]}}]
        self.data_req['requestOptions'] = {"property": "", "backend": "CM", "category": 0}
        self.data_value['hl'] = 'zh-CN'
        self.data_value['tz'] = '-480'
        self.data_value['req'] = urllib.request.quote(str(self.data_req))
        self.data_value['token'] = token
        for index in self.data_value:
            self.data_url = self.data_url + index + '=' + self.data_value[index] + '&'
        time.sleep(2)
        print("result_拼接后ur1", self.data_url)
        # 忽略ssl证书，重定向
        # self.proxies=
        results = requests.get(url=self.data_url, headers=self.data_headers, verify=False,
                               allow_redirects=False)
        time.sleep(5)
        page = results.content.decode("utf-8")
        jsonData = page[5:]
        data = json.loads(jsonData)
        items = data['default']['timelineData']
        sava_data = str([item["value"][0] for item in items])
        self.sava_data = sava_data
 
    def get_key_work(self):
        self.keywork = "China"
        redata = self.get_google_trend()
        print(redata)

 
if __name__ == '__main__':
    ts = TrendsSpider()
    ts.get_key_work()