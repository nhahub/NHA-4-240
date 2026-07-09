import { api } from '../api.js';

window.onload = () => {
    const button = document.getElementById('resetBtn');
    const oldPasswordInput = document.getElementById('oldPassword');
    const newPasswordInput = document.getElementById('newPassword');
    const confirmPasswordInput = document.getElementById('confirmPassword');
       const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    button.addEventListener('click',async() =>{
        const oldPassword = oldPasswordInput.value;
        const newPassword = newPasswordInput.value;
        const confirmPassword = confirmPasswordInput.value;
      
        const hasUpperCase=/[A-Z]/.test(newPassword);
        const hasLowerCase=/[a-z]/.test(newPassword);
        const hasNumber=/[0-9]/.test(newPassword);
        const hasSpecial=/[^a-zA-Z0-9]/.test(newPassword);
        const hasValidLength=newPassword.length <=12 && newPassword.length >= 8;

      

        if(!oldPassword || !newPassword || !confirmPassword){
         errorMsg.textContent = 'Please fill all required fields';
            errorMsg.style.display = 'block';
            return;
        }
        if(newPassword !== confirmPassword){
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

            await api.patch('Auth/resetPassword',{
                Password : oldPassword,
                newPassword : newPassword
            });

            successMsg.textContent = 'Password changed successfully!';
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
function goBack(){
    window.history.back();
}
window.goBack = goBack;
function setupPasswordToggle(inputId, toggleId) {
    const input = document.getElementById(inputId);
    const toggle = document.getElementById(toggleId);

    toggle.addEventListener("click", () => {
        const isPassword = input.type === "password";

        input.type = isPassword ? "text" : "password";

        toggle.classList.toggle("fa-eye", !isPassword);
        toggle.classList.toggle("fa-eye-slash", isPassword);
    });
}

setupPasswordToggle("oldPassword", "toggleOldPassword");
setupPasswordToggle("newPassword", "toggleNewPassword");
setupPasswordToggle("confirmPassword", "toggleConfirmPassword");