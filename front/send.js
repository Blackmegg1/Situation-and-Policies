function getAnswer(question) {
  let quest = document.getElementById("question");
  let answerArea = document.getElementById("answerArea");
  let backPromise = fetch("https://yesno.wtf/api", quest);
  backPromise
    .then((respon) => respon.json())
    .then((data) => (answerArea.value = data.answer));
}
