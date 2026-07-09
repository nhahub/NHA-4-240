
document.addEventListener("DOMContentLoaded", () => {

    const toggle = document.getElementById("themeToggle");
    const match = document.cookie.match(/theme=(light|dark)/);
    const  isLight = match && match[1] === "light";

        document.body.classList.toggle("light-theme",isLight);
        if (toggle) {
            toggle.checked = isLight;
        
    
        toggle.addEventListener("change", () => {
            const light = toggle.checked;
                document.body.classList.toggle("light-theme",light);

                document.cookie =`theme=${light ? "light" : "dark"}; path=/; max-age=31536000`;
            
        

        });
    }

});