from sklearn.feature_extraction.text import HashingVectorizer
# list of text documents
text = ["The quick brown fox jumped over the lazy dog."]
# create the transform
vectorizer = HashingVectorizer(n_features=15)
# encode document
vector = vectorizer.transform(text)
# summarize encoded vector
print(vector.shape)
print(vector.toarray())

text = ["The dog."]
vectorizer = HashingVectorizer(n_features=10)
vector = vectorizer.transform(text)
print(vector.shape)
print(vector.toarray())

text = ["The dog,dog"]
vectorizer = HashingVectorizer(n_features=20)
vector = vectorizer.transform(text)
print(vector.shape)
print(vector.toarray())
