import sys
import time

def add(a,b):
    return a+b

if __name__ == "__main__":
    print(sys.path)
    sys.stdout.flush()
    time.sleep(10);
    print(sys.argv[1])
    sys.stdout.flush()
    time.sleep(10);
    print(sys.argv[2])
    print(add(len(sys.argv[1]),len(sys.argv[2])))
    print("hello python")