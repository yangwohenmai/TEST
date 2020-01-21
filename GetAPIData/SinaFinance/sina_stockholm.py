#coding:utf-8
 
import requests
import json
import datetime
import timeit
import time
import io
import os
import csv
import re
from pymongo import MongoClient
from multiprocessing.dummy import Pool as ThreadPool
from functools import partial
 
class Stockholm(object):
 
    def __init__(self, args):
        ## --reload {Y,N}              是否重新抓取股票数据，默认值：Y
        self.reload_data = args.reload_data
        ##  --portfolio {Y,N}           是否生成选股测试结果，默认值：N
        self.gen_portfolio = args.gen_portfolio
        ## --output {json,csv,all}     输出文件格式，默认值：json
        self.output_type = args.output_type
        ## --charset {utf-8,gbk}       输出文件编码，默认值：utf-8
        self.charset = args.charset
        ## --testrange NUM             测试日期范围天数，默认值：50
        self.test_date_range = args.test_date_range
        ## --startdate yyyy-MM-dd      抓取数据的开始日期，默认值：当前系统日期-100天（例如2015-01-01）
        self.start_date = args.start_date
        ##  --enddate yyyy-MM-dd        抓取数据的结束日期，默认值：当前系统日期
        self.end_date = args.end_date
        ## --targetdate yyyy-MM-dd     测试选股策略的目标日期，默认值：当前系统日期
        self.target_date = args.target_date
        ## --thread NUM                线程数，默认值：10
        self.thread = args.thread
        ## --storepath PATH            输出文件路径，默认值：stockholm_export
        if(args.store_path == 'USER_HOME/tmp/stockholm_export'):
            self.export_folder = os.path.expanduser('~') + '/tmp/stockholm_export'
        else:
            self.export_folder = args.store_path
        ## --testfile PATH             测试文件路径，默认值：./portfolio_test.txt
        self.testfile_path = args.testfile_path
        ## 选股策略的字符串表达式
        self.methods = args.methods
 
        ## 导出的文件名称
        self.export_file_name = 'stockholm_export'
        #大盘指数
        self.index_array = ['sh000001', 'sz399001', 'sh000300']
        self.sh000001 = {'Symbol': 'sh000001', 'Name': '上证指数'}
        self.sz399001 = {'Symbol': 'sz399001', 'Name': '深证成指'}
        self.sh000300 = {'Symbol': 'sh000300', 'Name': '沪深300'}
        ## self.sz399005 = {'Symbol': '399005.SZ', 'Name': '中小板指'}
        ## self.sz399006 = {'Symbol': '399006.SZ', 'Name': '创业板指'}
 
        ## mongodb数据库的相关信息
        self.mongo_url = 'localhost'
        self.mongo_port = 27017
        self.database_name = args.db_name
        self.collection_name = 'testing_method'
    #获取csv文件存储时，第一行的列名
    def get_columns(self, quote):
        columns = []
        if(quote is not None):
            for key in quote.keys():
                if(key == 'Data'):
                    for data_key in quote['Data'][-1]:
                        columns.append("data." + data_key)
                else:
                    columns.append(key)
            columns.sort()
        return columns
 
    #比较两个价格的变化率
    def get_profit_rate(self, price1, price2):
        if(price1 == 0):
            return None
        else:
            return round((price2-price1)/price1, 5)
 
    #获取均值
    def get_MA(self, number_array):
        total = 0
        n = 0
        for num in number_array:
            if num is not None and num != 0:
                n += 1
                total += num
        return round(total/n, 3)
 
    #将选股策略字符串表达式，转化为python函数表达式
    def convert_value_check(self, exp):
        val = exp.replace('day', 'quote[\'Data\']').replace('(0)', '(-0)')
        val = re.sub(r'\(((-)?\d+)\)', r'[target_idx\g<1>]', val)
        val = re.sub(r'\.\{((-)?\w+)\}', r"['\g<1>']", val)
        return val
 
    def convert_null_check(self, exp):
        p = re.compile('\((-)?\d+...\w+\}')
        iterator = p.finditer(exp.replace('(0)', '(-0)'))
        array = []
        for match in iterator:
            v = 'quote[\'Data\']' + match.group()
            v = re.sub(r'\(((-)?\d+)\)', r'[target_idx\g<1>]', v)
            v = re.sub(r'\.\{((-)?\w+)\}', r"['\g<1>']", v)
            v += ' is not None'
            array.append(v)
        val = ' and '.join(array)
        return val
    #KDJ指数类
    class KDJ():
        def _avg(self, array):
            length = len(array)
            return sum(array)/length
 
        def _getMA(self, values, window):
            array = []
            x = window
            while x <= len(values):
                curmb = 50
                if(x-window == 0):
                    curmb = self._avg(values[x-window:x])
                else:
                    curmb = (array[-1]*2+values[x-1])/3
                array.append(round(curmb,3))
                x += 1
            return array
 
        def _getRSV(self, arrays):
            rsv = []
            x = 9
            while x <= len(arrays):
                high = max(map(lambda x: x['High'], arrays[x-9:x]))
                low = min(map(lambda x: x['Low'], arrays[x-9:x]))
                close = arrays[x-1]['Close']
                rsv.append((close-low)/(high-low)*100)
                t = arrays[x-1]['Date']
                x += 1
            return rsv
 
        #根据股票数据，计算KDJ指数
        def getKDJ(self, quote_data):
            if(len(quote_data) > 12):
                rsv = self._getRSV(quote_data)
                k = self._getMA(rsv,3)
                d = self._getMA(k,3)
                j = list(map(lambda x: round(3*x[0]-2*x[1],3), zip(k[2:], d)))
 
                for idx, data in enumerate(quote_data[0:12]):
                    data['KDJ_K'] = None
                    data['KDJ_D'] = None
                    data['KDJ_J'] = None
                for idx, data in enumerate(quote_data[12:]):
                    data['KDJ_K'] = k[2:][idx]
                    data['KDJ_D'] = d[idx]
                    if(j[idx] > 100):
                        data['KDJ_J'] = 100
                    elif(j[idx] < 0):
                        data['KDJ_J'] = 0
                    else:
                        data['KDJ_J'] = j[idx]
 
            return quote_data
 
    # 下载所有的股票编号，返回字典数组，每个元素都是股票编号字典
    def load_all_quote_symbol(self):
        print("开始下载所有的股票符号..." + "\n")
 
        start = timeit.default_timer()
 
        all_quotes = []
 
        all_quotes.append(self.sh000001)  #将大盘添加到最前面
        all_quotes.append(self.sz399001)  #将大盘添加到最前面
        all_quotes.append(self.sh000300)  #将大盘添加到最前面
        ## all_quotes.append(self.sz399005) #将大盘添加到最前面
        ## all_quotes.append(self.sz399006) #将大盘添加到最前面
 
        try:
            count = 1
            while (count < 100):
                para_val = '[["hq","hs_a","",0,' + str(count) + ',500]]'
                r_params = {'__s': para_val}
                all_quotes_url = 'http://money.finance.sina.com.cn/d/api/openapi_proxy.php'
                r = requests.get(all_quotes_url, params=r_params)  #根据网址下载所有的股票编号，这里使用的是新浪网址，有很多网址可以下载股票编号
                print("从  "+r.url+"  处下载所有的股票编号")
                if(len(r.json()[0]['items']) == 0):  #响应返回的是一个数组，自己打开网址一看便懂
                    break
                for item in r.json()[0]['items']:
                    quote = {}
                    code = item[0]  #股票编号
                    name = item[2]  #股票名称
                    # 获取股票编号后再进行每个股票的数据爬取，需要先对编号转换一下格式，后面我们使用腾讯股票接口，编号格式相同，所以这不需要进行转换
                    # if(code.find('sh') > -1):
                    #     code = code[2:] + '.SS'
                    # elif(code.find('sz') > -1):
                    #     code = code[2:] + '.SZ'
                    ## 将股票基本信息记入字典
                    quote['Symbol'] = code  #股票编号
                    quote['Name'] = name  #股票名称
                    all_quotes.append(quote)
                count += 1
        except Exception as e:
            print("Error: 下载股票编号失败..." + "\n")
            print(e)
 
        print("下载所有的股票编号完成... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
        return all_quotes
 
 
    # 根据指定股票编号，下载此股票的数据，这里使用腾信接口
    def load_quote_data(self, quote, start_date, end_date, is_retry, counter):
        # print("开始下载指定股票的数据..." + "\n")
 
        start = timeit.default_timer()
        # 直接从腾讯的js接口中读取
        if (quote is not None and quote['Symbol'] is not None):
            try:
                url = 'http://data.gtimg.cn/flashdata/hushen/latest/daily/' + quote['Symbol'] + '.js'  # 腾讯的日k线数据
                r = requests.get(url)  # 向指定网址请求，下载股票数据
                alldaytemp = r.text.split("\\n\\")[2:]  #根据返回的字符串进行处理提取出股票数据的数组形式
                quote_data=[]
                for day in alldaytemp:
                    if(len(day)<10):   #去掉一些不对的数据，这里去除方法比较笼统.
                        continue
                    oneday = day.strip().split(' ')  #获取日K线的数据。strip为了去除首部的\n，' '来分割数组，分割出来的数据分别是日期、开盘、收盘、最高、最低、成交量
                    onedayquote={}
                    onedayquote['Date'] = "20"+oneday[0]  #腾讯股票数据中时间没有20170513中的20，所以这里加上，方便后面比较
                    onedayquote['Open'] = oneday[1]  #开盘
                    onedayquote['Close'] = oneday[2]  #收盘
                    onedayquote['High'] = oneday[3]  #最高
                    onedayquote['Low'] = oneday[4] #最低
                    onedayquote['Volume'] = oneday[5] #成交量
                    quote_data.append(onedayquote)
                quote['Data'] = quote_data #当前股票每天的数据
                # print(quote_data)
                if (not is_retry):
                    counter.append(1)
 
            except:
                print("Error: 加载指定股票的数据失败... " + quote['Symbol'] + "/" + quote['Name'] + "\n")
                if (not is_retry):
                    time.sleep(2)
                    self.load_quote_data(quote, start_date, end_date, True,counter)  ##这里重试，以免是因为网络问题而导致的下载失败
 
            print("下载指定股票 " + quote['Symbol'] + "/" + quote['Name'] + " 完成..." + "\n")
            ## print("time cost: " + str(round(timeit.default_timer() - start)) + "s." + "\n")
            ## print("total count: " + str(len(counter)) + "\n")
        return quote
    #根据股票符号，下载所有的股票数据
    def load_all_quote_data(self, all_quotes, start_date, end_date):
        print("开始下载所有股票数据..." + "\n")
 
        start = timeit.default_timer()
 
        counter = []
        mapfunc = partial(self.load_quote_data, start_date=start_date, end_date=end_date, is_retry=False, counter=counter)  #创建映射函数
        pool = ThreadPool(self.thread)  # 开辟包含指定数目线程的线程池
        pool.map(mapfunc, all_quotes)   # 多线程执行下载工作
        pool.close()
        pool.join()
 
        print("下载所有的股票数据完成... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
        return all_quotes
 
    #股票数据处理
    def data_process(self, all_quotes):
        print("开始处理所有的股票..." + "\n")
 
        kdj = self.KDJ()
        start = timeit.default_timer()
 
        for quote in all_quotes:
            #划分股票类别
            if(quote['Symbol'].startswith('300')):
                quote['Type'] = '创业板'
            elif(quote['Symbol'].startswith('002')):
                quote['Type'] = '中小板'
            else:
                quote['Type'] = '主板'
 
            if('Data' in quote):  #Data字段中存储了股票所需要的所有数据
                try:
                    temp_data = []
                    for quote_data in quote['Data']:  #遍历当前股票每一天的数据
                        if(quote_data['Volume'] != '000' or quote_data['Symbol'] in self.index_array): #如果成交量不等于0或股票不是大盘指数才能正常处理
                            d = {}
                            d['Open'] = float(quote_data['Open'])  #开盘价
                            ## d['Adj_Close'] = float(quote_data['Adj_Close'])
                            d['Close'] = float(quote_data['Close'])  #收盘价
                            d['High'] = float(quote_data['High'])  #最高价
                            d['Low'] = float(quote_data['Low'])  #最低价
                            d['Volume'] = int(quote_data['Volume'])  #成交量
                            d['Date'] = quote_data['Date']  #时间字符串
                            temp_data.append(d)
                    quote['Data'] = temp_data
                except KeyError as e:
                    print("Data Process: Key Error")
                    print(e)
                    print(quote)
 
        ## 计算5天、10天、20天、30天均线
        for quote in all_quotes:
            if('Data' in quote):
                try:
                    for i, quote_data in enumerate(quote['Data']):
                        if(i > 0):
                            quote_data['Change'] = self.get_profit_rate(quote['Data'][i-1]['Close'], quote_data['Close'])  #计算股票变化率
                            quote_data['Vol_Change'] = self.get_profit_rate(quote['Data'][i-1]['Volume'], quote_data['Volume'])  #计算股票成交量变化率
                        else:
                            quote_data['Change'] = None
                            quote_data['Vol_Change'] = None
 
                    last_5_array = []
                    last_10_array = []
                    last_20_array = []
                    last_30_array = []
                    for i, quote_data in enumerate(quote['Data']):
                        last_5_array.append(quote_data['Close'])   #记录收盘价，用来计算股票k日均值
                        last_10_array.append(quote_data['Close']) #记录收盘价，用来计算股票k日均值
                        last_20_array.append(quote_data['Close']) #记录收盘价，用来计算股票k日均值
                        last_30_array.append(quote_data['Close']) #记录收盘价，用来计算股票k日均值
                        quote_data['MA_5'] = None
                        quote_data['MA_10'] = None
                        quote_data['MA_20'] = None
                        quote_data['MA_30'] = None
 
                        if(i < 4):  #不到5日，不开始计算
                            continue
                        if(len(last_5_array) == 5):
                            last_5_array.pop(0)
                        quote_data['MA_5'] = self.get_MA(last_5_array) #计算股票5日均值
 
                        if(i < 9): #不到10日不开始计算
                            continue
                        if(len(last_10_array) == 10):
                            last_10_array.pop(0)
                        quote_data['MA_10'] = self.get_MA(last_10_array) #计算股票10日均值
 
                        if(i < 19): #不到20日不开始计算
                            continue
                        if(len(last_20_array) == 20):
                            last_20_array.pop(0)
                        quote_data['MA_20'] = self.get_MA(last_20_array) #计算股票20日均值
 
                        if(i < 29): #不到30日不开始计算
                            continue
                        if(len(last_30_array) == 30):
                            last_30_array.pop(0)
                        quote_data['MA_30'] = self.get_MA(last_30_array) #计算股票30日均值
 
 
                except KeyError as e:
                    print("Key Error")
                    print(e)
                    print(quote)
 
        ## 计算KDJ指数
        for quote in all_quotes:
            if('Data' in quote):
                try:
                    kdj.getKDJ(quote['Data'])  #计算股票的KDJ指数
                except KeyError as e:
                    print("Key Error")
                    print(e)
                    print(quote)
 
        print("所有的股票处理结束... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
 
    #数据导出
    def data_export(self, all_quotes, export_type_array, file_name):
        print("开始导出"+str(len(all_quotes))+"个股票数据")
        start = timeit.default_timer()
        directory = self.export_folder
        if(file_name is None):
            file_name = self.export_file_name
        if not os.path.exists(directory):  #如果目录不存在
            os.makedirs(directory)   #创建目录
 
        if(all_quotes is None or len(all_quotes) == 0):
            print("没有数据要导出...\n")
 
        if('json' in export_type_array):  #如果导出类型中包含json，就导出json文件
            print("开始导出到json文件...\n")
            f = io.open(directory + '/' + file_name + '.json', 'w', encoding=self.charset)  #使用指定的字符编码写入文件
            json.dump(all_quotes, f, ensure_ascii=False)
 
        if('csv' in export_type_array):  #如果导出类型中包含csv，就导出csv文件
            print("开始导出到csv文件...\n")
            columns = []
            if(all_quotes is not None and len(all_quotes) > 0):
                columns = self.get_columns(all_quotes[0])  #获取csv文件第一行的列名
            writer = csv.writer(open(directory + '/' + file_name + '.csv', 'w', encoding=self.charset))  #使用指定的字符编码写入文件
            writer.writerow(columns)  #写入列头作为第一行
 
            for quote in all_quotes: #将数据一次写入每一行
                if('Data' in quote):
                    for quote_data in quote['Data']:
                        try:
                            line = []
                            for column in columns:
                                if(column.find('data.') > -1):
                                    if(column[5:] in quote_data):
                                        line.append(quote_data[column[5:]])
                                else:
                                    line.append(quote[column])
                            writer.writerow(line)
                        except Exception as e:
                            print(e)
                            print("write csv error: " + quote)
 
        if('mongo' in export_type_array):
            print("开始导出到 MongoDB数据库...\n")
 
        print("导出数据完成.. time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
 
    def file_data_load(self):
        print("开始从文件中加载数据..." + "\n")
 
        start = timeit.default_timer()
        directory = self.export_folder
        file_name = self.export_file_name
 
        f = io.open(directory + '/' + file_name + '.json', 'r', encoding=self.charset)
        json_str = f.readline()
        all_quotes_data = json.loads(json_str)
 
        print("文件中数据加载"+str(len(all_quotes_data))+"个股票完成... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
        return all_quotes_data
 
    #校验日期，检测date时间的股票数据是否已出爬出来了
    def check_date(self, all_quotes, date):
        is_date_valid = False
        for quote in all_quotes:
            if(quote['Symbol'] in self.index_array):  #选取大盘数据，是因为大盘数据绝对会有，这就要求你爬虫时一定要把大盘数据获取下来
                for quote_data in quote['Data']:  #获取一只股票每一天的数据行情
                    if(quote_data['Date'] == date):   #爬虫的股票数据中的时间和将要测试的时间要相同
                        is_date_valid = True
                        return is_date_valid
        if not is_date_valid:
            print(date + " 日期不存在数据...\n")
        return is_date_valid
 
    #根据选股策略，选择股票
    def quote_pick(self, all_quotes, target_date, methods):
        print(target_date+"选股启动..." + "\n")
 
        start = timeit.default_timer()
 
        results = []
        data_issue_count = 0  #有问题股票的数据个数
 
        for quote in all_quotes:
            try:
                if(quote['Symbol'] in self.index_array):  #如果是大盘数据，自动选中
                    results.append(quote)   #将大盘数据加入到选中集合
                    continue
 
                target_idx = None
                for idx, quote_data in enumerate(quote['Data']):  #迭代遍历股票的数据
                    if(quote_data['Date'] == target_date):  #首先判断股票是否包含目标时间的数据
                        target_idx = idx
                if(target_idx is None):  #如果股票不包含目标时间数据，就错过该股票
                    ## print(quote['Name'] + " data is not available at this date..." + "\n")
                    data_issue_count+=1
                    continue
 
                ## 选股策略 ##
                valid = False  #默认是无效的
                for method in methods:  #测试所有选股策略
                    ## print(method['name'])
                    ## null_check = eval(method['null_check'])
                    try:
                        value_check = eval(method['value_check'])   #执行选股策略（选股策略就是python代码的字符串表达式）
                        if(value_check):
                            quote['Method'] = method['name']  #如果选股策略有效，这种将测试方法名加入到股票数据中，为了记录这只股票是通过什么策略选中的
                            results.append(quote)  #将选中的股票加入到结果集中
                            valid = True
                            break
                    except:
                        valid = False
                if(valid):
                    continue
 
                ## 选股策略结束##
 
            except KeyError as e:
                ## print("KeyError: " + quote['Name'] + " data is not available..." + "\n")
                data_issue_count+=1
 
        print("选股完成... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
        print(str(data_issue_count) + " 个股票时间数据有问题...\n")
        return results
 
    #把根据选股策略选择的股票进行测试
    def profit_test(self, selected_quotes, target_date):
        print("启动股票策略测试..." + "\n")
 
        start = timeit.default_timer()
 
        results = []
        INDEX = None  #沪深300股票
        INDEX_idx = 0  #股票目标日期的数据在日k线中的索引
 
        for quote in selected_quotes:   #遍历每只选中的股票，找到沪深300指定日期的股票数据
            if(quote['Symbol'] == self.sh000300['Symbol']):  #如果是沪深300
                INDEX = quote
                for idx, quote_data in enumerate(quote['Data']):  #迭代股票每一天的数据，为了找到指定日期的数据
                    if(quote_data['Date'] == target_date):
                        INDEX_idx = idx
                break
 
        for quote in selected_quotes:
            target_idx = None
 
            if(quote['Symbol'] in self.index_array):  #去除大盘股票
                continue
 
            #获取指定时间在股票数据中的索引，去除不包含指定日期的股票，这么没必要，因为在选取的时候就是按照包含指定日期选取的
            for idx, quote_data in enumerate(quote['Data']):  #迭代股票每一天的数据
                if(quote_data['Date'] == target_date):
                    target_idx = idx
            if(target_idx is None):
                print(quote['Name'] + " 的股票数据不可处理..." + "\n")
                continue
 
            test = {}
            test['Name'] = quote['Name']  #股票名称
            test['Symbol'] = quote['Symbol']  #股票编号
            test['Method'] = quote['Method']  #选股策略
            test['Type'] = quote['Type']  #股票类型：创业板，中小板，主板
            if('KDJ_K' in quote['Data'][target_idx]):
                test['KDJ_K'] = quote['Data'][target_idx]['KDJ_K']  #kdj指数
                test['KDJ_D'] = quote['Data'][target_idx]['KDJ_D']  #kdj指数
                test['KDJ_J'] = quote['Data'][target_idx]['KDJ_J']  #kdj指数
            test['Close'] = quote['Data'][target_idx]['Close']  #收盘价
            test['Change'] = quote['Data'][target_idx]['Change'] #变化率
            test['Vol_Change'] = quote['Data'][target_idx]['Vol_Change'] #成交量变化率
            test['MA_5'] = quote['Data'][target_idx]['MA_5'] #5日均值
            test['MA_10'] = quote['Data'][target_idx]['MA_10'] #10日均值
            test['MA_20'] = quote['Data'][target_idx]['MA_20'] #20日均值
            test['MA_30'] = quote['Data'][target_idx]['MA_30'] #30日均值
            test['Data'] = [{}]
 
            for i in range(1,11):   #这里为什么用11
                if(target_idx+i >= len(quote['Data'])):  #如果预测日期超出数据范围，则结束
                    print(quote['Name'] + " 的数据在 "+target_date+"后" + str(i) + " 天的测试存在问题..." + "\n")
                    break
 
                day2day_profit = self.get_profit_rate(quote['Data'][target_idx]['Close'], quote['Data'][target_idx+i]['Close'])  #获取多日股价变化率
                test['Data'][0]['Day_' + str(i) + '_Profit'] = day2day_profit  #记录股价变化率数据
                if(INDEX_idx+i < len(INDEX['Data'])):
                    day2day_INDEX_change = self.get_profit_rate(INDEX['Data'][INDEX_idx]['Close'], INDEX['Data'][INDEX_idx+i]['Close'])
                    test['Data'][0]['Day_' + str(i) + '_INDEX_Change'] = day2day_INDEX_change
                    test['Data'][0]['Day_' + str(i) + '_Differ'] = day2day_profit-day2day_INDEX_change
 
            results.append(test)
 
        print("选股测试完成... time cost: " + str(round(timeit.default_timer() - start)) + "s" + "\n")
        return results
 
    #下载数据
    def data_load(self, start_date, end_date, output_types):
        all_quotes = self.load_all_quote_symbol()  #下载所有的股票符号
        print("共 " + str(len(all_quotes)) + " 股票符号下载完成..." + "\n")
        self.load_all_quote_data(all_quotes, start_date, end_date)  #下载所有的股票数据
        self.data_process(all_quotes)  #数据处理
 
        self.data_export(all_quotes, output_types, None)  #数据导出
 
 
    #数据预测
    def data_test(self, target_date, date_range, output_types):
        ## 加载选股策略
        methods = []
        path = self.testfile_path  #选股策略文件路径
 
        ## 从mongodb数据库加载选股策略
        if(path == 'mongodb'):
            print("从Mongodb加载测试方法...\n")
            client = MongoClient(self.mongo_url, self.mongo_port)
            db = client[self.database_name]
            col = db[self.collection_name]
            q = None
            if(len(self.methods) > 0):
                applied_methods = list(map(int, self.methods.split(',')))
                q = {"method_id": {"$in": applied_methods}}
            for doc in col.find(q, ['name','desc','method']):
                print(doc)
                m = {'name': doc['name'], 'value_check': self.convert_value_check(doc['method'])}
                methods.append(m)
 
        ## 从文件加载选股策略（文件要求utf8编码）
        else:
            if not os.path.exists(path):
                print("选股策略测试文件不存在，测试取消...\n")
                return
            f = io.open(path, 'r', encoding='utf-8')   #在保存选股策略文件时，记得保存成utf8编码格式的
            for line in f:  #每行一个选股策略
                if(line.startswith('##') or len(line.strip()) == 0):  #去除注释行
                    continue
                line = line.strip().strip('\n')
                name = line[line.find('[')+1:line.find(']:')]  #提取本次测试的代号，也就是每行最前面的[]里面的内容
                value = line[line.find(']:')+2:]  #提取[]:后的内容——选股策略
                m = {'name': name, 'value_check': self.convert_value_check(value)}
                methods.append(m)
 
        if(len(methods) == 0):
            print("没有发现测试方法，测试取消...\n")
            return
 
        ## 启动选股策略测试
        all_quotes = self.file_data_load()   #从文件中加载股票数据
        target_date_time = datetime.datetime.strptime(target_date, "%Y%m%d") #将目标时间字符串转化为时间对象
        for i in range(date_range):
            date = (target_date_time - datetime.timedelta(days=i)).strftime("%Y%m%d")  #将未来时间字符串转换为指定时间格式
            is_date_valid = self.check_date(all_quotes, date)  #校验时间
            if is_date_valid:
                selected_quotes = self.quote_pick(all_quotes, date, methods)  #根据选策略选取股票
                res = self.profit_test(selected_quotes, date)  #测试股票
                self.data_export(res, output_types, 'result_' + date)  #测试结果导出
 
    def run(self):
        ## 输出数据类型
        output_types = []
        if(self.output_type == "json"):
            output_types.append("json")
        elif(self.output_type == "csv"):
            output_types.append("csv")
        elif(self.output_type == "all"):
            output_types = ["json", "csv"]
 
        ## 根据选定的日期范围抓取所有沪深两市股票的行情数据。
        if(self.reload_data == 'Y'):
            print("开始下载股票数据...\n")
            self.data_load(self.start_date, self.end_date, output_types)
 
        # 测试和生成选择策略
        if(self.gen_portfolio == 'Y'):
            print("开始选股测试...\n")
            self.data_test(self.target_date, self.test_date_range, output_types)