function getAnswer(question) {
  let quest = document.getElementById("question");
  let answerArea = document.getElementById("answerArea");
  let backPromise = fetch("http://192.168.0.181:3000"+"?question="+quest.value);
  backPromise
    .then((respon) => respon.json())
    .then((data) => (answerArea.value = data.answer))
    .catch(error =>alert(error));
}
