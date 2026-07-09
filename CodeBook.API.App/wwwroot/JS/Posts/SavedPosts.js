import { api } from '../api.js';
   const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

document.addEventListener('DOMContentLoaded', () => {
    loadSavedPosts();
});

async function loadSavedPosts() {
    const container = document.getElementById('savedPostsContainer');

    try {
        const posts = await api.get('Post/saved');

        if (!posts || posts.length === 0) {
            container.innerHTML = `
                <div class="post-card" style="text-align:center; padding:40px">
                    <i class="fa-solid fa-bookmark" 
                       style="font-size:40px; color:#30363d; margin-bottom:15px"></i>
                    <p style="color:#8b949e">No saved posts yet!</p>
                    <a href="../Posts/Feed.html" class="btn-purple mt-2">
                        Browse Posts
                    </a>
                </div>`;
            return;
        }

        container.innerHTML = '';
        posts.forEach(post => {
            container.innerHTML += `
                <div class="post-card" id="post-${post.id}">
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
                        <button class="btn-purple" 
                                onclick="viewPost(${post.id})">
                            <i class="fa-solid fa-eye"></i> View Post
                        </button>
                        <button class="btn-purple" 
                                style="border-color:#f85149; color:#f85149"
                                onclick="unsavePost(${post.id})">
                            <i class="fa-solid fa-bookmark-slash"></i> Unsave
                        </button>
                    </div>
                </div>
            `;
        });

    } catch (error) {
        container.innerHTML = `
            <p style="color:#f85149">Failed to load saved posts. Are you logged in?</p>`;
        console.error(error);
    }
}

async function unsavePost(postId) {
    try {
        const result = await api.delete(`Post/${postId}/unsave`);

        if (result.message === 'Post unsaved successfully') {
            document.getElementById(`post-${postId}`).remove();

            const container = document.getElementById('savedPostsContainer');
            if (container.children.length === 0) {
                container.innerHTML = `
                    <div class="post-card" style="text-align:center; padding:40px">
                        <i class="fa-solid fa-bookmark" 
                           style="font-size:40px; color:#30363d; margin-bottom:15px"></i>
                        <p style="color:#8b949e">No saved posts yet!</p>
                        <a href="../Posts/Feed.html" class="btn-purple mt-2">
                            Browse Posts
                        </a>
                    </div>`;
            }
        }
    } catch (error) {
        console.error(error);
    }
}

function viewPost(postId) {
    window.location.href = `../Posts/PostDetail.html?id=${postId}`;
}

window.unsavePost = unsavePost;
window.viewPost = viewPost;