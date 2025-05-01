# 前言
傳統的方式是透過nlog把log送到cloudwatch, 長期觀察發現成本居高不下

收到建議可以把log改用firehose改放到s3, 可以有效的降低成本, 接著嘗試就開始了...

## 目標
1. 透過nlog把log送到 firehose
2. firehsoe 把資料轉換格式後送到s3
3. 透過athena 查詢s3的內容

## 步驟
大部分的操作都是在aws上進行, 有空再繼續補完

to be continued...
