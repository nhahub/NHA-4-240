import { api } from '../api.js';

let currentUser = null;

window.onload = async() =>{
    await loadProfile();
}
async function loadProfile(){
    try{
        const data = await api.get('User/viewmyprofile');
        currentUser = data;

        document.getElementById('username').innerText = data.userName;
        document.getElementById('bio').innerText = data.bio || 'No bio yet';
        document.getElementById('followersCount').innerText = data.followersCount;
        document.getElementById('followingCount').innerText = data.followingCount;

        const avatar = document.getElementById('avatar');
         avatar.src = data.avatarUrl || 'https://i.pinimg.com/originals/60/b6/6f/60b66f7e6337e0de45cea924a3946dbd.png';

     
    }
    catch(error){
        console.error('Failed to load Profile:',error);
        
    }
}

function showProfile(){
document.getElementById('content').innerHTML = `
        <div class="card p-4">
            <h4>My Profile</h4>
            <hr style="border-color:#30363d">
            <p><strong>Username:</strong> ${currentUser?.userName || ''}</p>
            <p><strong>Bio:</strong> ${currentUser?.bio || 'No bio yet'}</p>
    
        </div>
    `;
}
function showEdit() {
    document.getElementById('content').innerHTML = `
        <div class="card p-4">
            <h4>Edit Profile</h4>
            <hr style="border-color:#30363d">
            <div class="mb-3">
                <label class="form-label">Username</label>
                <input type="text" class="form-control" id="editUsername" value="${currentUser?.userName || ''}">
            </div>
            <div class="mb-3">
                <label class="form-label">Bio</label>
                <textarea class="form-control" id="editBio" rows="3">${currentUser?.bio || ''}</textarea>
            </div>
            <div class="mb-3">
                <label class="form-label">Avatar URL</label>
                <input type="text" class="form-control" id="editAvatar" value="${currentUser?.avatarUrl || ''}">
            </div>
            <button class="btn btn-purple w-100" onclick="saveProfile()">
                Save Changes
            </button>
        </div>
    `;
}
async function saveProfile(){
    try{
        const username = document.getElementById('editUsername').value;
        const bio = document.getElementById('editBio').value;
        const avatarUrl = document.getElementById('editAvatar').value;

        await api.patch('User/updatemyprofile',{
            UserName : username,
            Bio : bio,
            AvatarUrl : avatarUrl
        });

         alert('Profile updated successfully!!');
         window.location.reload();
    }
    catch(error){
        alert('Failed to update: '+error.message);
    }
}
window.showProfile = showProfile;
window.showEdit = showEdit;
window.saveProfile = saveProfile;

async function showPosts() {
    document.getElementById('content').innerHTML = `
        <div class="container">
            <div class="mt-3 mb-3">
                <h4 style="color:white">My Posts</h4>
                <hr style="border-color:#30363d">
            </div>
            <div id="postsList">
                <p style="color:#8b949e">Loading posts...</p>
            </div>
        </div>
    `;

    try {
        const posts = await api.get('Post/myposts');

        if (!posts || posts.length === 0) {
            document.getElementById('postsList').innerHTML = `
                <div style="text-align:center; padding:40px">
                    <i class="fa-solid fa-file-code" 
                       style="font-size:40px; color:#30363d; margin-bottom:15px"></i>
                    <p style="color:#8b949e">No posts yet!</p>
                </div>`;
            return;
        }

        const postsList = document.getElementById('postsList');
        postsList.innerHTML = '';

        posts.forEach(post => {
            postsList.innerHTML += `
                <div class="post-card">
                    <h2 class="post-title">${post.title}</h2>

                    <p style="color:#8b949e; font-size:13px">
                        ${new Date(post.dateCreated).toLocaleDateString()}
                        ${post.communityName ? `
                        • <a href="../Community/CommunityFeed.html?id=${post.communityId}"
                             style="color:#7c3aed; text-decoration:none">
                            <i class="fa-solid fa-people-group"></i> ${post.communityName}
                          </a>
                        ` : ''}
                    </p>

                    <p class="post-body">${post.body}</p>

                    ${post.codeSnippet ? `
                    <pre class="code-snippet"><code>${post.codeSnippet}</code></pre>
                    ` : ''}

                    ${post.language ? `
                    <span style="color:#bca1ec; font-size:13px; display:block; margin-top:8px">
                        <i class="fa-solid fa-code"></i> ${post.language}
                    </span>
                    ` : ''}

                    <div class="post-actions mt-3">
                        <button class="btn-purple" 
                                onclick="window.location.href='../Posts/PostDetail.html?id=${post.id}'">
                            <i class="fa-solid fa-eye"></i> View Post
                        </button>
                        <a href="../Posts/EditPosts.html?id=${post.id}" class="btn-purple">
                            <i class="fa-solid fa-pen"></i> Edit Post
                        </a>
                    </div>
                </div>
            `;
        });

    } catch (error) {
        document.getElementById('postsList').innerHTML = `
            <p style="color:#f85149">Failed to load posts!</p>
        `;
        console.error(error);
    }
}
window.showPosts = showPosts;

function showPassword() {
    window.location.href = '../Auth/ResetPassword.html';
}
window.showPassword = showPassword;

function showSaved() {
    window.location.href = '../Posts/SavedPosts.html';
}
async function showDelete() {
    const confirmed = confirm("Are you sure you want to delete your account? This cannot be undone!");
    
    if (confirmed) {
        try {
            await api.delete('User/deletemyprofile');
            alert("Account deleted successfully!");
            window.location.href = "../Auth/Login.html";
        } catch (error) {
            console.log(error.message);
            alert("Failed to delete: " + error.message);
        }
    }
}

window.showDelete = showDelete;

function showNotification() {
    window.location.href = 'Notifications.html';
}

window.showNotification = showNotification;

function showCommunity() {
    window.location.href = '../Community/Community.html';
}
function showMyCommunities(){
    window.location.href = '../Community/CommunityOwner.html';
}

function showFollowers() {
    window.location.href = 'Followers.html';
}

function showFollowing() {
    window.location.href = 'Followings.html';
}

window.showCommunity = showCommunity;
window.showFollowers = showFollowers;
window.showFollowing = showFollowing;
window.showMyCommunities = showMyCommunities;
window.showSaved = showSaved;

function escapeHTML(htmlString) {
    if(!htmlString) return "";
  return htmlString
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&#039;");
};

window.escapeHTML = escapeHTML;

function goBack() {
    window.history.back();
}
window.goBack = goBack;