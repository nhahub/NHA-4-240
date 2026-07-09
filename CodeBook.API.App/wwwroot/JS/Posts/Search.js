import { api } from '../api.js';

let currentTab = 'posts';
   const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

document.getElementById('searchInput').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') search();
});

document.getElementById('languageFilter').addEventListener('change', () => {
    search();
});


function switchTab(tab) {
    currentTab = tab;

    document.querySelectorAll('[id^="tab-"]').forEach(btn => {
        btn.style.backgroundColor = 'transparent';
        btn.style.color = '#7c3aed';
    });
    document.getElementById(`tab-${tab}`).style.backgroundColor = '#7c3aed';
    document.getElementById(`tab-${tab}`).style.color = 'white';

    if (tab === 'posts') {
        document.getElementById('postFilters').style.display = 'flex';
        document.getElementById('otherFilters').style.display = 'none';
    } else {
        document.getElementById('postFilters').style.display = 'none';
        document.getElementById('otherFilters').style.display = 'block';
    }

    document.getElementById('resultsContainer').innerHTML = 
        `<p style="color:#8b949e">Search for ${tab} above...</p>`;
    document.getElementById('resultsCount').style.display = 'none';

    const keyword = document.getElementById('searchInput').value.trim();
    if (keyword) search();
}

async function search() {
    const keyword = document.getElementById('searchInput').value.trim();
    const resultsContainer = document.getElementById('resultsContainer');
    const resultsCount = document.getElementById('resultsCount');

    resultsContainer.innerHTML = `<p style="color:white">Searching...</p>`;
    resultsCount.style.display = 'none';

    try {
        let results = [];

        if (currentTab === 'posts') {
            results = await searchPosts(keyword);
        } else if (currentTab === 'users') {
            results = await searchUsers(keyword);
        } else if (currentTab === 'communities') {
            results = await searchCommunities(keyword);
        }

        if (!results || results.length === 0) {
            resultsContainer.innerHTML = `
                <p style="color:#8b949e">
                    No ${currentTab} found. Try different keywords!
                </p>`;
            return;
        }

        resultsCount.textContent = `Found ${results.length} ${currentTab}`;
        resultsCount.style.display = 'block';

        resultsContainer.innerHTML = '';
        if (currentTab === 'posts') renderPosts(results);
        else if (currentTab === 'users') renderUsers(results);
        else if (currentTab === 'communities') renderCommunities(results);

    } catch (error) {
        resultsContainer.innerHTML = `
            <p style="color:#f85149">Search failed. Please try again.</p>`;
        console.error(error);
    }
}

async function searchPosts(keyword) {
    const language = document.getElementById('languageFilter').value;
    const tag = document.getElementById('tagFilter').value.trim();

    const params = new URLSearchParams();
    if (keyword) params.append('keyword', keyword);
    if (language) params.append('language', language);
    if (tag) params.append('tag', tag);

    return await api.get(`Post/search?${params.toString()}`);
}


async function searchUsers(keyword) {
    if (!keyword) return [];
    return await api.get(`User/search?keyword=${encodeURIComponent(keyword)}`);
}


async function searchCommunities(keyword) {
    if (!keyword) return [];
    return await api.get(`Communities/search?keyword=${encodeURIComponent(keyword)}`);
}


function renderPosts(posts) {
    const container = document.getElementById('resultsContainer');
    posts.forEach(post => {
        container.innerHTML += `
            <div class="post-card">
                <h2 class="post-title">${post.title}</h2>
                <p style="color:#8b949e; font-size:13px">
                    by ${post.authorUsername || 'Unknown'} • 
                    ${new Date(post.dateCreated).toLocaleDateString()}
                </p>
                <p class="post-body">${post.body}</p>
                ${post.codeSnippet ? `
                <pre class="code-snippet"><code>${post.codeSnippet}</code></pre>
                ` : ''}
                ${post.language ? `
                <span style="color:#bca1ec; font-size:13px">
                    <i class="fa-solid fa-code"></i> ${post.language}
                </span>
                ` : ''}
                <div class="post-actions mt-3">
                    <button class="btn-purple" onclick="viewPost(${post.id})">
                        <i class="fa-solid fa-eye"></i> View Post
                    </button>
                </div>
            </div>
        `;
    });
}

function renderUsers(users) {
    const container = document.getElementById('resultsContainer');
    users.forEach(user => {
        container.innerHTML += `
            <div class="post-card d-flex align-items-center gap-3">
                <img src="${user.avatarUrl? user.avatarUrl :'https://i.pinimg.com/originals/60/b6/6f/60b66f7e6337e0de45cea924a3946dbd.png'}" 
                     style="width:50px; height:50px; border-radius:50%; 
                            border:2px solid #7c3aed; object-fit:cover">
                <div>
                    <p  font-weight:bold; margin:0">
                        ${user.userName}
                    </p>
                    <p style="color:#8b949e; font-size:13px; margin:0">
                        ${user.bio || 'No bio yet'}
                    </p>
                </div>
                <button class="btn-purple ms-auto" 
                        onclick="viewProfile(${user.id})">
                    <i class="fa-solid fa-user"></i> View Profile
                </button>
            </div>
        `;
    });
}

// Render Communities
function renderCommunities(communities) {
    const container = document.getElementById('resultsContainer');
    communities.forEach(community => {
        container.innerHTML += `
            <div class="post-card d-flex align-items-center gap-3">
                <img src="${community.iconURL? community.iconURL : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png'}" 
                     style="width:50px; height:50px; border-radius:50%; 
                            border:2px solid #7c3aed; object-fit:cover">
                <div>
                    <p class ="community-name-search">
                        ${community.name}
                    </p>
                    <p style="color:#8b949e; font-size:13px; margin:0">
                        ${community.description || 'No description'}
                    </p>
                </div>
                <button class="btn-purple ms-auto" id ="view-community-btn" 
                        onclick="viewCommunity(${community.communityId})">
                    <i class="fa-solid fa-people-group"></i> View Community
                </button>
            </div>
        `;
    });
}

function viewPost(postId) {
    window.location.href = `PostDetail.html?id=${postId}`;
}

function viewProfile(userId) {
    window.location.href = `../../html/User/OtherUserProfile.html?userId=${userId}`;
}

async function viewCommunity(communityId,buttonElement) {
    const communities = await api.get("communities/getcommunities");
    var joined = false;
        if(communities && communities.length > 0){
        const validCommunityIds = new Set(communities.map(c => c.communityId));
        if(validCommunityIds.has(communityId)){
            window.location.href = `../../HTML/Community/CommunityFeed.html?id=${communityId}`;
            joined = true;
        }
       }
       if(!joined){
      errorMsg.textContent = 'Join Community to view posts!';
            errorMsg.style.display = 'block';
        window.location.href = `../../HTML/Community/Community.html`;
    }
}
document.addEventListener("DOMContentLoaded", () => {
    const params = new URLSearchParams(window.location.search);
    const tag = params.get("tag");
    const mention= params.get("mention");

     const searchInput = document.getElementById("searchInput");
     const tagFilter = document.getElementById("tagFilter");

    if (tag) {
        if (tagFilter) tagFilter.value = tag;
        if (searchInput) searchInput.value = tag;
        setTimeout(() => search(), 100);
    }
    else if(mention){

    switchTab("users");

    if (searchInput){
        searchInput.value = mention;
        
    setTimeout(() => search(), 100);
}
    }
});
window.switchTab = switchTab;
window.search = search;
window.viewPost = viewPost;
window.viewProfile = viewProfile;
window.viewCommunity = viewCommunity;