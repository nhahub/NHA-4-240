import { api } from '../api.js';

window.onload = async () => {
    var followersContainer = document.getElementById("followers-container");

   await loadFollowers(followersContainer);
};

async function loadFollowers(followersContainer) {
    try{
        const followers = await api.get('User/followers');

        if(!followers || followers.length===0){
            followersContainer.innerHTML =  '<span style="color: Red;">No followers found.</span>';
            return;
        }

        followersContainer.innerHTML = '';
        followers.forEach(user => {
            const followerCard = document.createElement('div');
            followerCard.className = 'follower-card';
            followerCard.innerHTML = `
                <a href="OtherUserProfile.html?userId=${user.id}" class="card p-4 text-decoration-none text-dark shadow-sm hover-card">
                <div class="row align-items-center">
                <div class="col-md-3 text-center">
                <img src="${user.avatarUrl ? user.avatarUrl : 'https://i.pinimg.com/originals/60/b6/6f/60b66f7e6337e0de45cea924a3946dbd.png'}"
                alt="Profile Avatar"
                class="profile-img">
                </div>
                <div class="col-md-9">
                <h2 class="follower-name mb-1">${user.userName}</h2>
                <p class="follower-bio-50">${user.bio || 'No bio yet'}</p>
                </div>
                </div>
                </a>`;
                followersContainer.appendChild(followerCard);
         
        });
}
catch(error){
     errorMsg.textContent = "Couldn't load followers" + error.message;
     errorMsg.style.display = 'block';
}
};

function goBack() {
    window.history.back();
}
window.goBack = goBack;