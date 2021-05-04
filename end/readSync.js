let fs = require("fs");
let rawdata = fs.readFileSync("answer.json");
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
  for (let i of answerArray) {
    if (i.Body.search(question) != -1) {
      return i;
    }
  }
}

function handleAnswer(answer) {
  let handled = [];
  for (let i of answer.Answer) {
    for (let j of answer.Options) {
      if (i == j.key) {
        handled.push(j.value);
      }
    }
  }
  return handled;
}

console.log(handleAnswer(searchAnswer("国民主联盟于2020年7月9日举行")));
