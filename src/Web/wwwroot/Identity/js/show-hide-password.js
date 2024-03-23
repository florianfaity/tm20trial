const togglePasswordButton = document.getElementById("toggle-password");
const togglePasswordMobileButton = document.getElementById("toggle-password-mobile");
const toggleConfirmPasswordButton = document.getElementById("toggle-confirm-password");
const toggleConfirmPasswordMobileButton = document.getElementById("toggle-confirm-password-mobile");

if (togglePasswordButton) {
    togglePasswordButton.addEventListener("click", togglePassword);
}

if (togglePasswordMobileButton) {
    togglePasswordMobileButton.addEventListener("click", togglePasswordMobile);
}

if (toggleConfirmPasswordButton) {
    toggleConfirmPasswordButton.addEventListener("click", toggleConfirmPassword);
}

if (toggleConfirmPasswordMobileButton) {
    toggleConfirmPasswordMobileButton.addEventListener("click", toggleConfirmPasswordMobile);
}

function togglePassword() {
    const passwordInput = document.getElementById("password");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        document.getElementById("img-eye-open").hidden = true;
        document.getElementById("img-eye-close").hidden = false;
    } else {
        passwordInput.type = "password";
        document.getElementById("img-eye-open").hidden = false;
        document.getElementById("img-eye-close").hidden = true;
    }
}

function togglePasswordMobile() {
    const passwordInput = document.getElementById("password-mobile");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        document.getElementById("img-eye-open-mobile").hidden = true;
        document.getElementById("img-eye-close-mobile").hidden = false;
    } else {
        passwordInput.type = "password";
        document.getElementById("img-eye-open-mobile").hidden = false;
        document.getElementById("img-eye-close-mobile").hidden = true;
    }
}

function toggleConfirmPassword() {
    const confirmPasswordInput = document.getElementById("confirm-password");
    if (confirmPasswordInput.type === "password") {
        confirmPasswordInput.type = "text";
        document.getElementById("img-eye-open-confirm").hidden = true;
        document.getElementById("img-eye-close-confirm").hidden = false;
    } else {
        confirmPasswordInput.type = "password";
        document.getElementById("img-eye-open-confirm").hidden = false;
        document.getElementById("img-eye-close-confirm").hidden = true;
    }
}

function toggleConfirmPasswordMobile() {
    const confirmPasswordInput = document.getElementById("confirm-password-mobile");
    if (confirmPasswordInput.type === "password-mobile") {
        confirmPasswordInput.type = "text";
        document.getElementById("img-eye-open-mobile-confirm").hidden = true;
        document.getElementById("img-eye-close-mobile-confirm").hidden = false;
    } else {
        confirmPasswordInput.type = "password";
        document.getElementById("img-eye-open-mobile-confirm").hidden = false;
        document.getElementById("img-eye-close-mobile-confirm").hidden = true;
    }
}
