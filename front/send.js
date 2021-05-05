function getAnswer(question) {
  let quest = document.getElementById("question");
  let answerArea = document.getElementById("answerArea");
  // let backPromise = fetch("http://192.168.0.181:3000"+"?question="+quest.value);
  if(quest.value==""){
    alert("题目不能为空");
    return false;
  }
  let backPromise = fetch(
    "http://106.15.249.229:3000" + "?question=" + quest.value
  );
  backPromise
    .then((respon) => respon.json())
    .then((data) => (answerArea.value = data.answer))
    .catch((error) => alert(error));
  backPromise = null;
  quest.value = "";
}

function enter_btn(a) {
  //监听答案区的回车事件
  if (a == 13) {
    getAnswer();
  }
}

document.addEventListener("paste", function (event) {
  //监听全局的粘贴事件
  let clipText = event.clipboardData.getData("Text");
  let quest = document.getElementById("question");
  quest.value = clipText;
  getAnswer();
});
