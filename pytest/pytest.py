
import requests
import datetime
from baidu_id import province,city
 
def getIndex(word="我和我的祖国"):
    """
        搜索指数
        :param word:
        :return:
        """
    url = f"http://index.baidu.com/api/SearchApi/index?word={word}&area=0&days=30"
    rep_json = get_rep_json(url)
    generalRatio = rep_json['data']['generalRatio']
    uniqid = rep_json['data']['uniqid']
    all_index_e = rep_json['data']['userIndexes'][0]['all']['data']
    pc_index_e = rep_json['data']['userIndexes'][0]['pc']['data']
    wise_index_e = rep_json['data']['userIndexes'][0]['wise']['data']
    t = getPtbk(uniqid)
    startDate = rep_json['data']['userIndexes'][0]['wise']['startDate']
    all_news = getTopNews(decrypt_py(t, all_index_e), startDate, word)
    pc_news = getTopNews(decrypt_py(t, pc_index_e), startDate, word)
    wise_news = getTopNews(decrypt_py(t, wise_index_e), startDate, word)
    for each in (all_news, pc_news, wise_news):
        print(each)
    return None
 
 
def getFeedIndex(word="我和我的祖国"):
    """
    :param word: 关键词
    :return: 资讯指数
    """
    url="http://index.baidu.com/api/FeedSearchApi/getFeedIndex?word=%s&area=0&days=30"%word
    feed_index_data=get_rep_json(url)
    uniqid=feed_index_data['data']['uniqid']
    data=feed_index_data["data"]['index'][0]
    generalRatio=data['generalRatio']#资讯指数概览
    e=data['data']
    t=getPtbk(uniqid)
 
    return decrypt_py(t,e)
 
 
def getNewsDate(word):
    """
    :param word:
    :return: 媒体指数的峰顶新闻
    """
    url = f"http://index.baidu.com/api/NewsApi/getNewsIndex?area=0&word={word}&days=30"
    res_json = get_rep_json(url)['data']
 
    generalRatio = res_json["index"][0]['generalRatio']
    e = res_json['index'][0]['data']
    start_date = res_json['index'][0]['startDate']
    t = getPtbk(res_json['uniqid'])
 
    news=getTopNews(decrypt_py(t, e),start_date,word)
 
    return news
 
 
def getTopNews(numList:list,start_date,word):
    """
    找到当前指数列表中的峰值
    转换成日期字符串
    将合成的日期字符串带入到请求数据接口中
    返回新闻数据
    :param numList: 指数列表
    :param start_date: 起始日期
    :param word:
    :return: 峰值新闻
    """
    start_date = string_toDatetime(start_date)
    hill_tops = getHilltop(numList)
    hill_tops_date = [datetime_toString(start_date + datetime.timedelta(days=index)) for index in hill_tops]
    news = getNews(",".join(hill_tops_date), word)["data"][word]
 
    return news
 
 
def getNews(dts,word):
    """
    获取媒体指数接口数据
    :param dts:用,连接的时间字符串，例：dts=2019-10-06,2019-10-10,2019-10-12,2019-10-16,2019-10-21,2019-10-24
    :param word:
    :return:接口传回的数据
    """
    url=f"http://index.baidu.com/api/NewsApi/checkNewsIndex?dates[]={dts}&type=day&words={word}"
    return get_rep_json(url)
 
 
def getHilltop(numList: list):
    """
    :param numList:一组数值数组
    :return:峰值的序号列表
    """
    numList = list(map(lambda x: float(x) if x else 0, numList))
    hillTops = [index for index, each in enumerate(numList) if
                index and index < len(numList) - 1 and each > numList[index - 1] and each > numList[index + 1]]
 
    return hillTops
 
 
def getMulti(word="我和我的祖国"):
    """需求图谱
    pv搜索热度；ratio搜索变化率；sim相关性
    """
    url=f"http://index.baidu.com/api/WordGraph/multi?wordlist%5B%5D={word}"
    word_data=get_rep_json(url)['data']['wordlist'][0]
    if word_data['keyword']:
        print(word_data['wordGraph'])
 
 
def getRegion(word="我和我的祖国",startDate='2019-09-17',endDate='2019-10-17'):
    """地域分布"""
    url=f"http://index.baidu.com/api/SearchApi/region?region=0&word={word}&startDate={startDate}&endDate={endDate}"
    region=get_rep_json(url)['data']['region'][0]
    region_city=[{'city':city[int(city_n)],'number':region['city'][city_n]}for city_n in region['city']]
    region_prov=[{'prov':province[int(prov_n)],'number':region['prov'][prov_n]}for prov_n in region['prov']]
    print(region_city,region_prov)
 
def getBaseAttributes(word="我和我的祖国"):
    """人群属性"""
    url=f"http://index.baidu.com/api/SocialApi/baseAttributes?wordlist[]={word}"
    rep_data=get_rep_json(url)['data']['result']
    return rep_data
 
def getInterest(word="我和我的祖国"):
    """兴趣分布"""
    url=f"http://index.baidu.com/api/SocialApi/interest?wordlist[]={word}"
    rep_data = rep_data=get_rep_json(url)['data']['result']
    return rep_data
 
def string_toDatetime(string):
    # 把字符串转成datetime
    return datetime.datetime.strptime(string, "%Y-%m-%d")
def datetime_toString(dt):
    # 把datetime转成字符串
    return dt.strftime("%Y-%m-%d")
def getPtbk(uniqid):
    url=f"http://index.baidu.com/Interface/ptbk?uniqid={uniqid}"
    return get_rep_json(url)['data']
def decrypt_py(t,e):
    """
    :param t:
    :param e:
    :return: 解析出来的数据
    """
    a=dict()
    length=int(len(t)/2)
    for o in range(length):
        a[t[o]] = t[length + o]
    r="".join([a[each]for each in e ]).split(",")
 
    return r
def get_rep_json(url):
    """
    获取json
    :param url: 请求接口
    :return:
    """
    hearder = {
        'passport_headers':{
        'Host': 'passport.baidu.com',
        'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)',
        'Refer': 'https://www.baidu.com/?tn=62095104_7_oem_dg'

    }
    }
    response = requests.get(url, headers=hearder)
    response_data = response.json()
    print(response_data)
    return response_data
 
 
def main():
    getFeedIndex()
    getNewsDate()
    getIndex()
    getRegion()
    getBaseAttributes()
    getInterest()
 
if __name__=="__main__":
    main()