import { api } from '../api.js';
window.onload=()=>{
    var communitiesContainer = document.getElementById("communities-container");
    var explorecommunities = document.getElementById("explore-communities");
   const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    async function GetCommunities() {
        try{
            const communities = await api.get('communities/getCommunities');
            if(!communities || communities.length === 0){
                communitiesContainer.innerHTML = '<span style="color: Red;">No Commmunities found.</span>';
                return; 
            }
            communitiesContainer.innerHTML = '';
            communities.forEach(community => {
                const communityCard = document.createElement('div');
                communityCard.innerHTML = `
                <a href="Communityfeed.html?id=${community.communityId}" class="text-decoration-none text-dark">
                <div class="row align-items-center community-card p-3 ">
                <div class="col-md-3 text-center align-items-center">
                
               <img src="${community.iconURL ? community.iconURL : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png'}" 
                alt="Community Icon" 
                class="profile-img">
                </div>

                <div class="col-md-7">
                <h3 class="community-name  mb-1">
                <span id="name-display">${community.name}</span>
                <input type="text" id="name-input" class="form-control d-none" value="${community.name}">
                </h3>

                <p class="community-description" >
                <span id="desc-display">${community.description}</span>
                <textarea id="desc-input" class="form-control d-none">${community.description}</textarea>
                </p>

                <div class="d-flex gap-4"style="color:#bca1ec;">
                <small><strong>Creation Date: </strong>${new Date(community.dateCreated).toLocaleDateString()}</small>
                <small><strong>Members count: </strong>${community.memberscount}</small>
            </div>
            </div>
            </div>
             </a>`;
            communitiesContainer.appendChild(communityCard);
                
            });
        }
         catch(error){
            errorMsg.textContent = "Couldn't load communities: " + error.message;
            errorMsg.style.display = 'block';
        }
        
    }
     async function ExploreCommunities(){
         try{
            const communities = await api.get('communities/getunjoinedcommunities');
            if(!communities || communities.length === 0){
                explorecommunities.innerHTML = '<span style="color: Red;">No Commmunities found.</span>';
                return; 
            }
            explorecommunities.innerHTML = '';
            communities.forEach(community => {
                const communityCard = document.createElement('div');
                communityCard.innerHTML = `
                <div class="row align-items-center community-card pt-4">
                <div class="col-md-3 text-center align-items-center">
                
               <img src="${community.iconURL ? community.iconURL : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png'}" 
                alt="Community Icon" 
                class="profile-img">
                </div>

                <div class="col-md-7">
                <h3 class="community-name mb-1">
                <span id="name-display">${community.name}</span>
                <input type="text" id="name-input" class="form-control d-none" value="${community.name}">
                </h3>

                <p class="community-description" >
                <span id="desc-display">${community.description}</span>
                <textarea id="desc-input" class="form-control d-none">${community.description}</textarea>
                </p>

                <div class="d-flex gap-4"style="color:#bca1ec;">
                <small><strong>Creation Date: </strong>${new Date(community.dateCreated).toLocaleDateString()}</small>
                <small><strong>Members count: </strong>${community.memberscount}</small>
            </div>
            </div>
             <div class="actions m-3 col-12 text-center">
                    <button class="btn-purple mb-3 w-50 join-btn">
                    Join Community
                    </button>
                </div>
            </div>`;
            communityCard.querySelector('.join-btn').addEventListener('click', (e) => {
                        handleAction(community.communityId, e.target);});
            explorecommunities.appendChild(communityCard);
                
            });
        }
         catch(error){
            errorMsg.textContent = "Couldn't load communities: " + error.message;
            errorMsg.style.display = 'block';
        }


     }

    GetCommunities();
    ExploreCommunities();
};

window.handleAction = async (communityId ,buttonElement) => {
            buttonElement.disabled = true;
            try{
                const role = 'Member';
                const result = await api.post(`communities/${communityId}/joincommunity`,{Role : role});
                if(result.message ==="Joined Community Successfully"){
                    successMsg.textContent = "Joined Community";
                    successMsg.style.display = 'block';
                    window.location.href = `../../HTML/Community/Communityfeed.html?id=${communityId}`;
                }
            }
        catch(error){
            errorMsg.textContent = "Error: " + error.message;
            errorMsg.style.display = 'block';
            buttonElement.disabled = false;
        }

}

function goBack() {
    window.history.back();
}
window.goBack = goBack;