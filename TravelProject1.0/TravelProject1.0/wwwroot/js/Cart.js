if (document.readyState == "loading") {
    document.addEventListener("DOMContentLoaded", start);
} else {
    start();
}

function start() {
    removeItem();
    totalPrice();
}

function removeItem() {
    /*刪除*/
    var btnDelete = document.querySelectorAll(".btn-delete");
    for (var i = 0; i < btnDelete.length; i++) {
        var btnRemove = btnDelete[i];
        btnRemove.addEventListener("click", removeCartItem);
    }
}

/*刪除商品*/
function removeCartItem() {
    var cart = document.getElementsByClassName("cart")[0];
    var inpCheckbox = document.getElementsByClassName("inp-cart");
    var items = cart.getElementsByClassName("cart-item");
    for (var i = 0; i < items.length; i++) {
        if (inpCheckbox[i].checked == true) items[i].remove();
    }
}

/*總金額*/
function totalPrice() {
    var cart = document.getElementsByClassName("cart")[0];
    var cartItems = cart.getElementsByClassName("cart-item");
    var inpCheckbox = document.getElementsByClassName("inp-cart");
    var total = 0;
    for (var i = 0; i < cartItems.length; i++) {
        var cartItem = cartItems[i];
        var priceElement = cartItem.getElementsByClassName("cart-price")[0];
        var quantityElement = cartItem.getElementsByClassName("cart-quantity")[0];
        var price = priceElement.innerText;
        console.log(price);
        var quantity = quantityElement.innerText;
        console.log(quantity);
        if (inpCheckbox[i].checked == true) {
            total = total + (price * quantity);
            document.getElementsByClassName("total-price")[0].innerText = "NT" + total;
        } else {
            total = 0;
            document.getElementsByClassName("total-price")[0].innerText = "NT" + total;
        }
    }
}