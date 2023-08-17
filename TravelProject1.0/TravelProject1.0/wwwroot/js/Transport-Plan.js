var Plan = document.querySelector(".plan");
var Caption = document.querySelector(".caption");
var Describe = document.querySelector(".describe");
var Rating = document.querySelector(".rating");
var Page1 = document.querySelector(".page-1");
var Page2 = document.querySelector(".page-2");
var Page3 = document.querySelector(".page-3");
var Page4 = document.querySelector(".page-4");
var Page5 = document.querySelector(".page-5");

Page1.addEventListener("click", () => {
    Plan.style.display = "";
    Caption.style.display = "";
    Describe.style.display = "";
    Rating.style.display = "";
});
Page2.addEventListener("click", () => {
    Plan.style.display = "flex";
    Caption.style.display = "none";
    Describe.style.display = "none";
    Rating.style.display = "none";
});
Page3.addEventListener("click", () => {
    Caption.style.display = "flex";
    Plan.style.display = "none";
    Describe.style.display = "none";
    Rating.style.display = "none";
});
Page4.addEventListener("click", () => {
    Describe.style.display = "flex";
    Plan.style.display = "none";
    Caption.style.display = "none";
    Rating.style.display = "none";
});
Page5.addEventListener("click", () => {
    Rating.style.display = "flex";
    Plan.style.display = "none";
    Caption.style.display = "none";
    Describe.style.display = "none";
});