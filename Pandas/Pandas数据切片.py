import csv
import os
import numpy as np
import random
import requests
import pandas as pd

# 数据集名称
birth_weight_file = 'birth_weight.csv'

# download data and create data file if file does not exist in current directory
# 如果当前文件夹下没有birth_weight.csv数据集则下载dat文件并生成csv文件
if not os.path.exists(birth_weight_file):
    birthdata_url = 'https://github.com/nfmcclure/tensorflow_cookbook/raw/master/01_Introduction/07_Working_with_Data_Sources/birthweight_data/birthweight.dat'
    birth_file = requests.get(birthdata_url)
    birth_data = birth_file.text.split('\r\n')
    # split分割函数,以一行作为分割函数，windows中换行符号为'\r\n',每一行后面都有一个'\r\n'符号。
    birth_header = birth_data[0].split('\t')
    # 每一列的标题，标在第一行，即是birth_data的第一个数据。并使用制表符作为划分。
    birth_data = [[float(x) for x in y.split('\t') if len(x) >= 1] for y in birth_data[1:] if len(y) >= 1]
    print(np.array(birth_data).shape)
    # (189, 9)
    # 此为list数据形式不是numpy数组不能使用np,shape函数,但是我们可以使用np.array函数将list对象转化为numpy数组后使用shape属性进行查看。
    with open(birth_weight_file, "w",encoding='utf-8', newline='') as f:
    # with open(birth_weight_file, "w") as f:
        writer = csv.writer(f)
        writer.writerows([birth_header])
        writer.writerows(birth_data)
        f.close()
birth_data = []
with open(birth_weight_file) as csvfile:
    csv_reader = csv.reader(csvfile)  # 使用csv.reader读取csvfile中的文件
    print(csv_reader)
    birth_header = next(csv_reader)  # 读取第一行每一列的标题
    print(birth_header)
    for row in csv_reader:  # 将csv 文件中的数据保存到birth_data中
        birth_data.append(row)
        print(row)


birth_data = [[float(x) for x in row] for row in birth_data]  # 将数据从string形式转换为float形式


birth_data = np.array(birth_data)  # 将list数组转化成array数组便于查看数据结构
print(birth_data)
birth_header = np.array(birth_header)
print(birth_header)
print(birth_data.shape)  # 利用.shape查看结构。
print(birth_header.shape)






print(pd.Series([1,3,5,np.nan,6,8]))
dates = pd.date_range('20130101', periods=6)
print(dates)
#通过numpy array创建DataFrame,以datetime做为索引，并赋以列标签
print(pd.DataFrame(np.random.randn(6,4), index=dates, columns=list('ABCD')))
df = pd.DataFrame(np.random.randn(6,4), index=dates, columns=list('ABCD'))
df2 = pd.DataFrame({ 'A' : 1.,
                     'B' : pd.Timestamp('20130102'),
                     'C' : pd.Series(1,index=list(range(4)),dtype='float32'),
                     'D' : np.array([3] * 4,dtype='int32'),
                     'E' : pd.Categorical(["test","train","test","train"]),
                     'F' : 'foo' })
print(df2)
print(df.head(2))
print(df.columns)
print(df.values)
print(df.describe())
print(df.T)
print(df.sort_index(axis=1, ascending=False))#按照一个轴的索引排序
print(df.sort_values(by='B'))#按照数据排序







df = pd.read_csv('birth_weight.csv')  # 读取训练数据
df['NEW'] = df['LOW']-df['AGE'] #创建一列新数据
print(df.head())

print(df.describe(include='all'))#显示一些统计数据，最大最小值
print(df.T)                     # 数据转置
print(df.ix[:,1].head())        #取第一列所有数据
print(df.ix[0:25,:])            #逗号左右代表行数和列数.冒号左右两边代表行的取值范围，显示0-25行的所有数据

#print(df.drop(df.ix[0:25,:].rows[[1, 2]], axis = 0).head())
print(df.drop(df.ix[0:25,:].columns[[1, 2]], axis = 1).head())#删除数据中间的某几列
print(df.ix[10:20, 0:3])        # 显示10-20行的0-3列数据
print(df.ix[10:20, ['LOW','AGE','LWT']])
print(df.sort_values(by='LWT').head(15)) #按照某一列排序
print(df.dtypes)                # 显示每个字段的数据类型
print(df.index)                 #显示索引起始点和步长




#测试用例1
# region 测试用例1
csv_data = pd.read_csv('birth_weight.csv')  # 读取训练数据，第一行作为头
print(csv_data)
print(pd.read_csv('birth_weight.csv', header=None)) #不把第一行作为头
#print(csv_data.to_csv('birth_weight1.csv', header=False, index=False))#生成csv的时候不加行头和索引
#print(csv_data.to_csv('birth_weight2.csv', header=True, index=True))#生成csv的时候加行头和索引
print(csv_data.columns)#显示列字段
print(csv_data.columns.tolist())#把列字段转换为一个列表类型
print(csv_data["AGE"])
print(csv_data['AGE'].max()) # 获取一列中的最大值
print(csv_data[["AGE","LOW"]]) # 获取两列值
csv_data['NEW'] = csv_data['LOW']-csv_data['AGE'] # 新增一列数据
print(csv_data["LOW"]*csv_data["AGE"]) # 两列数据进行计算
print(csv_data.sort_values("BWT")) # 按照某一列排序
print(csv_data.sort_values("BWT", inplace=True, ascending=False)) # 按照某一列倒序排列，inplace=True替换原数组,返回None
print(csv_data)
print(csv_data.head(1)['BWT']) # 获取第一行的date列
print(csv_data.sort_values("BWT", inplace=True, ascending=True)) # 按照某一列正序排列，inplace=True替换原数组,返回None
print(csv_data)
print(csv_data.head(5)['BWT']) # 获取前5行的BWT列
print(csv_data.head(5)['BWT'][3]) # 获取前5行数据的，BWT列的第3个元素值
print(sum(csv_data['BWT'])) # BWT列求和
print(csv_data[csv_data['BWT'] == 1135.0]) # 获取符合这个条件的行,注意字符串和数字的区别
print(csv_data[csv_data['BWT'] == 1135.0].index[0]) # 获取符合这个条件的行索引号
print(csv_data.index) # 获取符合这个条件的行索引号
print(csv_data.index[3]) # 获取符合这个条件的行索引号
print(csv_data.index[5]) # 返回倒数第5行索引号
print(csv_data.index[-1]) # 返回倒数第一行索引号
print(csv_data.loc[6])   # 返回行标号为6的数据，即第七行值
print(csv_data.loc[3:6])# 返回行标号3-6行的数据
print(csv_data.loc[[1,2,6]])# 返回行索引号为1，2，6的值
print(csv_data.loc[[1,2,6]]['BWT'])# 返回行标号为1，2，6的,BWT列的值
print(csv_data.loc[0:5,'BWT'])# 返回行标号为0~5的,BWT列的值
print(csv_data.loc[0,'BWT'])#表示取第二行，BWT列的值
print(csv_data.loc[:,['LOW','BWT']]) # 获取所有行的LOW和BWT列的值
print(csv_data.loc[1,['LOW','BWT']]) # 获取第二行的LOW和BWT列的值
print(csv_data.loc[0:5,:]) #返回0-5行的所有列，不能返回一个列的范围（如3~4列）
print(csv_data.iloc[0]) # 获取第一行
print(csv_data.iloc[0:5,0:5]) # 获取前5行前5列的值
print(csv_data.iloc[[1,2,4],[0,2]]) # 获取第1，2，4行中的0，2列的数据
print(csv_data.iloc[[1,2,4],0:3]) # 获取第1，2，4行中的0~3列的数据
#print((csv_data[2] > 1).any()) # 对于Series应用any()方法来判断是否有符合条件的
print(csv_data.at[0,'BWT']) #表示取第二行，BWT列的值，和上面的方法类似
#endregion








print(csv_data.shape)  # (189, 9)
csv_batch_data = csv_data.tail(5)  # 取后5条数据
print(csv_batch_data) 
print(csv_data.head(5)) #取前5条
print(pd.read_csv('birth_weight.csv',nrows =10))#取前10条
print("-----")
print(pd.read_csv('birth_weight.csv',skiprows=3,nrows=5))#取从第3行开始取，取行数据条
print(pd.read_csv('birth_weight.csv').ix[3:5, 0:8])
print(csv_batch_data.shape)  # (5, 9)


