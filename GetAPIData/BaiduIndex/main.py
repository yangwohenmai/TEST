from get_index import BaiduIndex
import requests

if __name__ == "__main__":
    #测试
    url = "http://i.baidu.com"
    wcookie = {"BDUSS":"h2ckFlaGREVTFFNGlOV05IbXRXbFk0UGJsdkFJN2xMQTRrbDVCV005UFRRMGRlSUFBQUFBJCQAAAAAAAAAAAEAAADKhegA2P3D-9DWAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANO2H17Tth9eUT"}
    HTML = requests.get(url,cookies=wcookie).content
    print(HTML)

    #正式
    keywords = ['伊朗']
    baidu_index = BaiduIndex(keywords, '2019-08-01', '2020-01-21')
    for index in baidu_index.get_index():
        print(index)