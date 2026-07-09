import { api } from '../api.js';
import { parseText } from '../Interactions/HashTags.js';
console.log("Feed.js loaded");

const urlParams = new URLSearchParams(window.location.search);
let currentPage = parseInt(urlParams.get('page')) || 1;
const postsContainer = document.getElementById("postsContainer");
const badge = document.querySelector(".badge");
const logoutbtn = document.querySelector(".sidebar-logout");
   const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

logoutbtn.addEventListener('click',() =>{
    logout();
})

document.addEventListener("DOMContentLoaded", () => {
    getnotificationcount();
    loadFeed();
});
async function loadFeed() {
    console.log("loadFeed called!");
    console.trace(); 
    try {
        postsContainer.innerHTML = `<p style="color:white">Loading...</p>`;
        const data = await api.get(`Post/feed?page=${currentPage}`);
        const communities = await api.get(`communities/getCommunities`);
        var posts =[];
        console.log(communities.length);
        if(communities && communities.length > 0){
            const validCommunityIds = new Set(communities.map(c => c.communityId));
             posts = data.filter(p => { return (p.communityId == null) || (validCommunityIds.has(p.communityId))});
       }
        else {
            posts = data.filter(p => { return (p.communityId == null) || (!p.communityId)});
        }

        if (!posts || posts.length === 0) {
            postsContainer.innerHTML = `<p style="color:white">No posts found.</p>`;
            return;
        }
        
        postsContainer.innerHTML = "";
        posts.forEach(post => {
            const div=document.createElement("div");
            div.className="post-card"
            div.innerHTML = `
            
                    <h2 class="post-title">${post.title}</h2>
                    <p style="color:#8b949e;font-size:13px">
                        by ${post.authorUsername} • 
                        ${new Date(post.dateCreated).toLocaleDateString()}

                        ${post.communityName ? `
                        • <a href="../../HTML/Community/CommunityFeed.html?id=${post.communityId}"
                             style="color:#7c3aed; text-decoration:none">
                            <i class="fa-solid fa-people-group"></i> ${post.communityName}
                         </a>
                        ` : ''}
                    </p>
                   <p class="post-body">${parseText(post.body)}</p>

                    ${post.codeSnippet ? `
                    <pre class="code-snippet"><code>${post.codeSnippet}</code></pre>
                    ` : ''}

                    ${post.language ? `
                    <span style="color:#bca1ec;font-size:13px">
                        <i class="fa-solid fa-code"></i> ${post.language}
                    </span>
                    ` : ''}

                    <div class="post-actions">
                        <div class="d-flex gap-1">
                           <div class="post-reaction-total-box " id="reaction-count-${post.id}">
                 ${post.likeCount || 0}
                  </div>
                            <button class="btn-purple reaction-btn" 
                            data-post-id="${post.id}"
                              data-type="Like"
                              data-liked="false"
                              onclick="toggleReaction(this, ${post.id}, 'Like')">
                                👍
                            </button>
                            <button class="btn-purple reaction-btn" 
                              data-post-id="${post.id}"
                              data-type="Like"
                              data-liked="false"
                              onclick="toggleReaction(this, ${post.id}, 'Haha')">
                                😂
                            </button>
                            <button class="btn-purple reaction-btn" 
                              data-post-id="${post.id}"
                              data-type="Like"
                              data-liked="false"
                              onclick="toggleReaction(this, ${post.id}, 'love')">
                                   
                                ❤️
                            </button>
                        </div>

                        <button class="btn-purple" onclick="viewPost(${post.id})">
                            <i class="fa-solid fa-eye"></i> View Post
                        </button>
                    </div>

            `;
            postsContainer.appendChild(div);
            if (post.userReaction) {
    const btn = div.querySelector(
        `[onclick="toggleReaction(this, ${post.id}, '${post.userReaction}')"]`
    );

    if (btn) {
        btn.classList.add("reacted");
        btn.dataset.liked = "true";
    }
}
        });

        console.log('Current page:', currentPage);
        // update page number
        document.getElementById('pageNumber').textContent = `Page ${currentPage}`;

        // show/hide prev button on page 1
        document.getElementById('prevBtn').style.display = 
            currentPage === 1 ? 'none' : 'inline-block';

        document.getElementById('nextBtn').style.display = 
            posts.length < 20 ? 'none' : 'inline-block';

            

    } catch (error) {
        postsContainer.innerHTML = `
            <p style="color:red">Failed to load posts. Please try again.</p>
        `;
        console.error(error);
    }
}

window.addEventListener('popstate', () => {
    const params = new URLSearchParams(window.location.search);
    currentPage = parseInt(params.get('page')) || 1;
    loadFeed();
});

function changePage(direction) {
    if (currentPage + direction < 1) return;
    currentPage += direction;
    window.history.pushState({}, '', `Feed.html?page=${currentPage}`);
    window.scrollTo(0, 0);
    loadFeed();
}

function viewPost(postId) {
    window.location.href = `../../HTML/Posts/PostDetail.html?id=${postId}`;
}
window.viewPost = viewPost;

async function getnotificationcount() {
    try{
        const countResponse = await api.get("notification/getunreadcount");
        console.log(`${countResponse.unreadcount}`);
        if(countResponse.unreadCount > 0){
            badge.classList.remove('d-none');
            badge.textContent = countResponse.unreadCount;
            if(countResponse.unreadCount > 99) badge.textContent = "+99";
        }
        else{
            badge.classList.add('d-none');
        }

    }
    catch(error){
        badge.classList.add('d-none');
        console.log(error);
    }
    
}

async function logout() {
    localStorage.clear();
    const result = await api.delete('Auth/logout');
    if(result.message === "Logout Successful!"){
        alert("Logout Successful!");
    }
    window.location.href = '../Auth/Login.html';
}

window.changePage = changePage;