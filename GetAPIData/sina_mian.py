#coding:utf-8
#爬虫参数配置
import argparse 
import datetime
from sina_stockholm import Stockholm
import os
 
#获取偏移指定天数的时间表达式
def get_date_str(offset):
    if(offset is None):
        offset = 0
    date_str = (datetime.datetime.today() + datetime.timedelta(days=offset)).strftime("%Y%m%d")
    return date_str
 
 
 
_default = dict(
    reload_data = 'Y',                      # --reload {Y,N}              是否重新抓取股票数据，默认值：Y
    gen_portfolio = 'Y',                    # --portfolio {Y,N}           是否生成选股测试结果，默认值：N
    output_type = 'csv',                   # --output {json,csv,all}     输出文件格式，默认值：json
    charset = 'utf-8',                      # --charset {utf-8,gbk}       输出文件编码，默认值：utf-8
    test_date_range = 60,                    # --testrange NUM             测试日期范围天数，默认值：50
    start_date = get_date_str(-90),          # --startdate yyyy-MM-dd      抓取数据的开始日期，默认值：当前系统日期-100天（例如2015-01-01）
    end_date = get_date_str(None),          # --enddate yyyy-MM-dd        抓取数据的结束日期，默认值：当前系统日期
    target_date = get_date_str(None),       # --targetdate yyyy-MM-dd     测试选股策略的目标日期，默认值：当前系统日期
    store_path = 'stockholm_export',      # --storepath PATH            输出文件路径，默认值：stockholm_export
    thread = 10,                            # --thread NUM                线程数，默认值：10
    testfile_path = './portfolio_test.txt',# --testfile PATH             选股策略文件路径，默认值：./portfolio_test.txt
    db_name = 'stockholm',                  #选股策略数据库名称
    methods = ''                            #选股策略表达式
    )
 
parser = argparse.ArgumentParser(description='A stock crawler and portfolio testing framework.') 
parser.add_argument('--reload', type=str, default=_default['reload_data'], dest='reload_data', help='Reload the stock data or not (Y/N), Default: %s' % _default['reload_data'])
parser.add_argument('--portfolio', type=str, default=_default['gen_portfolio'], dest='gen_portfolio', help='Generate the portfolio or not (Y/N), Default: %s' % _default['gen_portfolio'])
parser.add_argument('--output', type=str, default=_default['output_type'], dest='output_type', help='Data output type (json/csv/all), Default: %s' % _default['output_type'])
parser.add_argument('--charset', type=str, default=_default['charset'], dest='charset', help='Data output charset (utf-8/gbk), Default: %s' % _default['charset'])
parser.add_argument('--testrange', type=int, default=_default['test_date_range'], dest='test_date_range', help='Test date range(days): %s' % _default['test_date_range'])
parser.add_argument('--startdate', type=str, default=_default['start_date'], dest='start_date', help='Data loading start date, Default: %s' % _default['start_date'])
parser.add_argument('--enddate', type=str, default=_default['end_date'], dest='end_date', help='Data loading end date, Default: %s' % _default['end_date'])
parser.add_argument('--targetdate', type=str, default=_default['target_date'], dest='target_date', help='Portfolio generating target date, Default: %s' % _default['target_date'])
parser.add_argument('--storepath', type=str, default=_default['store_path'], dest='store_path', help='Data file store path, Default: %s' % _default['store_path'])
parser.add_argument('--thread', type=int, default=_default['thread'], dest='thread', help='Thread number, Default: %s' % _default['thread'])
parser.add_argument('--testfile', type=str, default=_default['testfile_path'], dest='testfile_path', help='Portfolio test file path, Default: %s' % _default['testfile_path'])
parser.add_argument('--dbname', type=str, default=_default['db_name'], dest='db_name', help='MongoDB DB name, Default: %s' % _default['db_name'])
parser.add_argument('--methods', type=str, default=_default['methods'], dest='methods', help='Target methods for back testing, Default: %s' % _default['methods'])




 
def checkFoldPermission(path):
    try:
        if not os.path.exists(path):
            os.makedirs(path)
        else:
            txt = open(path + os.sep + "test.txt","w")
            txt.write("test")
            txt.close()
            os.remove(path + os.sep + "test.txt")

    except Exception as e:
        print(e)
        return False
    return True
 
def main():
    args = parser.parse_args()
    if not checkFoldPermission(args.store_path):  #检测是否具有读写存储文件的权限
        print('\n没有文件读写权限: %s' % args.store_path)
    else:
        print('股票数据爬虫和预测启动...\n')
        stockh = Stockholm(args)  #初始化参数
        stockh.run()  #启动
        print('股票数据爬虫和预测完成...\n')
 
if __name__ == '__main__':
    main()