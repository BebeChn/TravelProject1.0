
    const heartIcon = document.getElementById("heart-icon");

    heartIcon.addEventListener("click", function() {
    if (heartIcon.classList.contains("red")) {
        heartIcon.classList.remove("fa-regular","red");
        heartIcon.classList.add("fa-solid","red2"); 
    } else {
        heartIcon.classList.add("fa-solid", "red2");
        heartIcon.classList.remove("fa-regular","red");
    }
  });
