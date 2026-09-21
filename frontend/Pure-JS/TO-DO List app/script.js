const inputTask = document.getElementById("input-task");
const taskList = document.getElementById("task-list");
const btn = document.querySelector(".btn");

function addTask() {

    let taskValue = inputTask.value.trim();
    if (!taskValue) {
        alert("You must add a value!");
        return;
    }
    let li = document.createElement("li");
    let span = document.createElement("span");
    li.textContent = taskValue;
    span.innerHTML = "&times;";
    li.appendChild(span);
    taskList.appendChild(li);
    inputTask.value = "";
}

btn.addEventListener("click", addTask);

taskList.addEventListener("click", function (e) {
    if (e.target.tagName === "SPAN") {
        e.target.parentElement.remove();
    }
    else if (e.target.tagName === "LI") {
        e.target.classList.toggle("checked");
    }
});