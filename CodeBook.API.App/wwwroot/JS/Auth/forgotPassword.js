import { api } from '../api.js';

window.onload = () => {
    const button = document.getElementById('resetBtn');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
       const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    button.addEventListener('click',async() =>{
        const email = emailInput.value;
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;
        const hasUpperCase=/[A-Z]/.test(password);
        const hasLowerCase=/[a-z]/.test(password);
        const hasNumber=/[0-9]/.test(password);
        const hasSpecial=/[^a-zA-Z0-9]/.test(password);
        const hasValidLength=password.length <=12 && password.length >= 8;

      

        if(!password || !confirmPassword){
         errorMsg.textContent = 'Please fill all required fields';
            errorMsg.style.display = 'block';
            return;
        }
        if(password !== confirmPassword){
            errorMsg.textContent = 'Passwords do not match';
            errorMsg.style.display = 'block';
            return;
        }
        if(!hasUpperCase ){
             errorMsg.textContent = 'Password must contain at least one Upper character';
            errorMsg.style.display = 'block';
             return;
        }
           if(!hasLowerCase){
             errorMsg.textContent = 'Password must contain at least one Lower character';
            errorMsg.style.display = 'block';
             return;
        }
             if(!hasNumber){
             errorMsg.textContent = 'Password must contain at least one digit';
            errorMsg.style.display = 'block';
             return;
        }
            if(!hasSpecial){
             errorMsg.textContent = 'Password must contain at least one Special character ';
            errorMsg.style.display = 'block';
             return;
        }
               if(!hasValidLength){
             errorMsg.textContent = 'Password must contain at least 8 characters and at most 12 characters ';
            errorMsg.style.display = 'block';
             return;
        }
        try{
            button.innerText = "Resetting...";
            button.disabled = true;

            await api.post("Auth/forgotPassword",{
                Email:email,
                NewPassword:password
            });

             successMsg.textContent = 'Password reset successfully!';
            successMsg.style.display = 'block';
            window.location.href = "../../HTML/Auth/Login.html";
        }
        catch(error){
          errorMsg.textContent = "Reset failed: " + error.message;
            errorMsg.style.display = 'block';
        }finally{
            button.innerText = "Confirm";
            button.disabled = false;
        }
    });
}
function setupPasswordToggle(inputId, toggleId) {
    const input = document.getElementById(inputId);
    const toggle = document.getElementById(toggleId);

    toggle.addEventListener("click", () => {
        if (input.type === "password") {
            input.type = "text";
            toggle.classList.replace("fa-eye", "fa-eye-slash");
        } else {
            input.type = "password";
            toggle.classList.replace("fa-eye-slash", "fa-eye");
        }
    });
}

setupPasswordToggle("password", "togglePassword");
setupPasswordToggle("confirmPassword", "toggleConfirmPassword");