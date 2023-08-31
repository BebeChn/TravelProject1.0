document.addEventListener("DOMContentLoaded", function () {
    var resetPasswordButton = document.getElementById("resetPasswordButton");
    resetPasswordButton.addEventListener("click", function () {
        var emailInput = document.getElementById("emailInput");
        var emailInputValue = emailInput.value.trim();
        var verificationCodeInput = document.getElementById("verificationCode");
        var verificationCodeValue = verificationCodeInput.value.trim();

        if (verificationCodeValue == "" && emailInputValue == "") {
            alert("驗證碼或電子郵件未填寫");
            return false;
        }
        else
        { 
            vueApp.verifyCode();
        }
    });

})