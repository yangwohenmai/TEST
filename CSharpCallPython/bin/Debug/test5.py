import math
import sys
import time

def Quadratic_Equations(stra,strb,strc):
    a=float(stra)
    b=float(strb)
    c=float(strc)
    D=b**2-4*a*c
    ans=[]
    ans.append((-b+math.sqrt(D))/(2*a))
    ans.append((-b-math.sqrt(D))/(2*a))
    print(ans[0])
    print(ans[1])
    #return str(ans[0])

Quadratic_Equations(sys.argv[1],sys.argv[2],sys.argv[3])