import { api } from '../api.js';
window.onload=()=>{
    var communitydatacard = document.getElementById("community-data-holder");
    var communityfeed = document.getElementById("community-feed");
    var createpostbtn = document.getElementById("createpost-btn");
    const params = new URLSearchParams(window.location.search);
    const communityId = params.get("id");
       const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');
    console.log(communityId);

    createpostbtn.addEventListener('click',() => {
            window.location.href = `../../html/Posts/CreatePost.html?communityId=${communityId}`;
    });

    async function CommunityDataView() {
        try{
            const community = await api.get(`communities/${communityId}/getcommunity`);
            if(community){
                communitydatacard.innerHTML = `
                <div class="row align-items-center justify-content-center">
                <div class="col-md-3 text-center align-items-center">
                
               <img src="${community.iconURL ? community.iconURL : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png'}" 
                alt="Community Icon" 
                class="profile-img-2">
                </div>
      
                <div class="col-md-7">
                <h3 class="community-name  mb-1">
                <span id="name-display">${community.name}</span>
                </h3>

                <p class="community-description" >
                <span id="desc-display">${community.description}</span>
                </p>
        
                <div class="d-flex gap-4"style="color:#bca1ec;">
                <small><strong>Creation Date: </strong>${new Date(community.dateCreated).toLocaleDateString()}</small>
                <small><strong>Members count: </strong>${community.memberscount}</small>
                <small><strong>Owner ID: </strong>${community.ownerId}</small>
            </div>
            </div>
                <div class="actions m-3 col-12 text-center">
                    <button type="button" class="btn btn-outline-danger unjoin-btn d-none">Unjoin Community</button>
                </div>
                     
            </div>
                `;
                if(!community.isOwner) communitydatacard.querySelector('.unjoin-btn').classList.remove('d-none');
                 communitydatacard.querySelector('.unjoin-btn').addEventListener('click', (e) => {
                        handleAction(communityId, e.target);});
            }
        }
        catch(error){
               errorMsg.textContent = "Couldn't load community details: " + error.message;
            errorMsg.style.display = 'block';
        }
        
    }
    async function GetCommunityFeed() {
        try {
                communityfeed.innerHTML = `<p>Loading...</p>`;
                const posts = await api.get(`communities/${communityId}/getCommunityFeed`);
                if (!posts || posts.length === 0) {
                communityfeed.innerHTML = `<p>No posts found.</p>`;
                return;
                }

        communityfeed.innerHTML = "";
        posts.forEach(post => {
            communityfeed.innerHTML += `
                <div class="post-card">
                    <h2 class="post-title">${post.title}</h2>
                    <p style="color:#8b949e;font-size:13px">
                        by ${post.authorUsername} • 
                        ${new Date(post.dateCreated).toLocaleDateString()}
                    </p>
                    <p class="post-body">${post.body}</p>

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
                </div>
            `;
        });

    } catch (error) {
        communityfeed.innerHTML = `
            <p style="color:red">Failed to load posts. Please try again.</p>
        `;
        console.error(error);
    }
}


function viewPost(postId) {
    window.location.href = `../../HTML/Posts/PostDetail.html?id=${postId}`;
}

 CommunityDataView();
 GetCommunityFeed();

};

window.handleAction = async (communityId,buttonElement) => {
            buttonElement.disabled = true;
            try{
                const result = await api.delete(`communities/${communityId}/unjoin`);
                if(result.message ==="Unjoined Community Successfully"){
                   successMsg.textContent = "Unjouned Community!";
                   successMsg.style.display = 'block';
                    window.location.href = '../../HTML/Posts/Feed.html';
                }
            }
        catch(error){
        errorMsg.textContent = "Error: " + error.message;
        errorMsg.style.display = 'block';
            buttonElement.disabled = false;
        }

}

function viewPost(postId) {
    window.location.href = `../../HTML/Posts/PostDetail.html?id=${postId}`;
}
window.viewPost = viewPost;

function goBack() {
    window.history.back();
}
window.goBack = goBack;