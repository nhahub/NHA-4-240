import { api } from '../api.js';


let viewedUserId = null;

window.onload = async() =>{
    const params = new URLSearchParams(window.location.search);
    viewedUserId = params.get('userId');
    const username = params.get('username');

    if (!viewedUserId && username) {
        const user = await api.get(`User/findByUsername?username=${username}`);
        if (user) viewedUserId = user.id;
    }
    await loadProfile();
}
async function loadProfile(){
    try{
        const data = await api.get(`User/viewprofile?userId=${viewedUserId}`);

        document.getElementById('username').innerText = data.userName;
        document.getElementById('bio').innerText = data.bio || 'No bio yet';
        document.getElementById('followersCount').innerText = data.followersCount;
        document.getElementById('followingCount').innerText = data.followingCount;

        const avatar = document.getElementById('avatar');
        avatar.src = data.avatarUrl? data.avatarUrl : 'https://i.pinimg.com/originals/60/b6/6f/60b66f7e6337e0de45cea924a3946dbd.png';

     
    }
    catch(error){
        console.error('Failed to load Profile:',error);
        
    }
}
async function follow(){
    try{
        await api.post(`User/follow?userid=${viewedUserId}`);
          successMsg.textContent = "Followed!!";
        successMsg.style.display = 'block';
        await loadProfile();
    }
       catch(error){
         errorMsg.textContent = "Failed to follow: " + error.message;
            errorMsg.style.display = 'block';
    }
}
async function unfollow(){
    try{
        await api.delete(`User/unfollow?userid=${viewedUserId}`);
       successMsg.textContent = "Unfollowed!!";
            successMsg.style.display = 'block';
        await loadProfile();
    }
       catch(error){
        errorMsg.textContent = "Failed to unfollow: " + error.message;
            errorMsg.style.display = 'block';
    }
}

async function viewPosts() {
    const params = new URLSearchParams(window.location.search);
    viewedUserId = params.get('userId');
    const username = params.get('username');

    if (!viewedUserId && username) {
        const user = await api.get(`User/findByUsername?username=${username}`);
        if (user) viewedUserId = user.id;
    }

    document.getElementById('content').innerHTML = `
            <div class ="container">
            <div class ="mt-5"> 
                <h4>Posts</h4>
                <hr style="border-color:#30363d">
                </div>
                <div id="postsList" class="flex-grow-1">
                    <p style="color:#8b949e">Loading posts...</p>
                </div>
            </div>
        `;
    
        try {
            const data = await api.get('Post/feed?page=1');
           
            const posts = data.filter (p => (p.authorId === Number(viewedUserId)) && (p.communityId === null));
    
            if (posts.length === 0) {
                document.getElementById('postsList').innerHTML = `
                    <p style="color:#8b949e">No posts yet!</p>
                `;
                return;
            }
    
            document.getElementById('postsList').innerHTML = posts.map(post => `
                <div class = "mt-3 mb-3 post-card">
                <a class="text-decoration-none" href= "../Posts/PostDetail.html?id=${post.id}" >
                    <h5 class="post-title">${post.title}</h5>
                    <p class="post-body">${post.body}</p>
                    ${window.escapeHTML(post.codeSnippet)? `
                        <pre class="code-snippet"><code>${post.codeSnippet}</code></pre>
                    ` : ''}
                    <small style="color:#8b949e">
                        ${new Date(post.dateCreated).toLocaleDateString()}
                    </small>
                    ${post.communityId ? `
                        <a href= "../Community/CommunityFeed.html?id=${post.communityId}" style ="font-size:13px">Community</a>
                        ` : ''}
                </a></div>
            `).join('');
    
        } catch (error) {
            console.log(error.message);
            document.getElementById('postsList').innerHTML = `
                <p style="color:#f85149">Failed to load posts!</p>
            `;
        }
    
}

function goBack() {
    window.history.back();
}

window.goBack = goBack;
window.follow = follow;
window.unfollow = unfollow;
window.viewPosts = viewPosts;

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