const fs = require("fs");
let rawdata = fs.readFileSync("end/answer.json");
let answer = JSON.parse(rawdata);
let answerArray = Array.from(answer); //json对象数组,元素形式如下：
//  {
//   Body: '《中国共产党纪律处分条例》的修订是将党的纪律建设理论、实践和制度创新的最新成果，以党规党纪形式固定下来，从而提高纪律建设的（）、（）、（）。',
//   Answer: [ 'A', 'B', 'C' ],
//   Options: [
//     { key: 'B', value: '时代性' },
//     { key: 'D', value: '思想性' },
//     { key: 'C', value: '针对性' },
//     { key: 'A', value: '政治性' },
//     { key: 'E', value: '时效性' }
//   ]
// }

function searchAnswer(question) {
  //寻找答案
  for (let i of answerArray) {
    if (i.Body.search(question) != -1) {
      return i;
    }
  }
  return false;//没有找到题目则返回false
}

function handleAnswer(answer) {
  //处理答案
  let handled = [];
  if(answer){
  for (let i of answer.Answer) {
    for (let j of answer.Options) {
      if (i == j.key) {
        handled.push(j.value);
        handled.push("\r\n")
      }
    }
  }
}
else {
  handled.push("没有找到这题");
}
  return {"answer": handled };
}

//创建http服务器
const http = require("http");
const url = require("url");
const querystring = require("querystring");
http
  .createServer((req, res) => {
    let reqUrl = url.parse(req.url);
    let reqQuery = querystring.parse(reqUrl.query);//响应内容
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.writeHead(200, {
      "Content-Type": "text/html;charset=utf-8", //解决中文乱码问题
    });
    // console.log(reqQuery.question);
    let answer = handleAnswer(searchAnswer(reqQuery.question));
    console.log(JSON.stringify(answer))
    res.write(JSON.stringify(answer))
    // res.write(JSON.stringify(reqQuery)); //响应内容
    res.end(); //结束响应
  })
  .listen(3000); //开始监听本地3000端口
console.log(`http server is listening at http://localhost:3000`);

// console.log(
//   handleAnswer(searchAnswer("党和国家机构职能体系是我们党治国理政的重要保障"))
// );
