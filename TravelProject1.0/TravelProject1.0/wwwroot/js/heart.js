
  const heartIcon = document.getElementById("heart-icon");
  let isSolid = false;

    heartIcon.addEventListener("click", function () {
    if (isSolid) {
        heartIcon.classList.remove("fa-solid", "red2");
        heartIcon.classList.add("fa-regular", "red");
    } else {
        heartIcon.classList.remove("fa-regular", "red");
        heartIcon.classList.add("fa-solid", "red2");
    }
    isSolid = !isSolid;
})


