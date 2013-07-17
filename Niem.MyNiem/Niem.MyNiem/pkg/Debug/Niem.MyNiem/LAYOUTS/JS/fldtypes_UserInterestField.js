
function LikeUnlikeItem(itemId, listId, webId, flag, hlkLike, lblLikesCount) {
    try {
        LikeUnlikeOperation(itemId, listId, webId, flag, hlkLike, lblLikesCount);
    }
    catch (ex) {
    }
    return false;
}

function onObjectDeleted(sender, args) {
    UpdateUI(this);
}

function onQuerySucceeded(sender, args) {
    var isLike = this.flag;
    var enumerator = this.listItems.getEnumerator();
    if (enumerator.moveNext()) {
        //already liked
        if (!isLike) {
            //delete item
            var deletItemId = enumerator.get_current().get_item("ID");
            var ctx = new SP.ClientContext('/');
            this.web = ctx.get_web();
            this.list = this.web.get_lists().getByTitle("User Interest List");
            this.itemToBeDeleted = list.getItemById(parseInt(deletItemId));
            itemToBeDeleted.deleteObject();
            ctx.executeQueryAsync(Function.createDelegate(this, this.onObjectDeleted), Function.createDelegate(this, this.onQueryFailed));
        }
    }
    else {
        if (isLike) {
            //add record
            var ctx = new SP.ClientContext('/');
            this.web = ctx.get_web();
            this.list = this.web.get_lists().getByTitle("User Interest List");
            var listItemCreationInfo = new SP.ListItemCreationInformation();
            var newItem = list.addItem(listItemCreationInfo);
            var userLoginName = this.currentUser.get_loginName();

            newItem.set_item('ItemID', this.itemId);
            newItem.set_item('ListID', this.listId);
            newItem.set_item('WebID', this.webId);
            newItem.set_item('User', SP.FieldUserValue.fromUser(userLoginName));
            newItem.update();
            ctx.executeQueryAsync(Function.createDelegate(this, this.onItemAdded), Function.createDelegate(this, this.onQueryFailed));
        }
        else {
            //do nothing as item is already not present so no need to delete the item
        }
    }
}

function onItemAdded(sender, args) {
    UpdateUI(this);
}

function UpdateUI(obj) {
    var ctx = new SP.ClientContext('/');
    this.web = ctx.get_web();
    this.list = this.web.get_lists().getByTitle("User Interest List");
    var query = new SP.CamlQuery();
    var queryXML = '<View>' +
	    '<Query>' +
		    '<Where>' +
				    '<And>' +
					    '<And>' +
						    '<Eq>' +
							    '<FieldRef Name="WebID"/><Value Type="Text">' + webId + '</Value>' +
						    '</Eq>' +
						    '<Eq>' +
							    '<FieldRef Name="ListID"/><Value Type="Text">' + listId + '</Value>' +
						    '</Eq>' +
					    '</And>' +
					    '<Eq>' +
						    '<FieldRef Name="ItemID"/><Value Type="Text">' + itemId + '</Value>' +
					    '</Eq>' +
				    '</And>' +
		    '</Where>' +
	    '</Query>' +
	    '<ViewFields>' +
		    '<FieldRef Name="User"/>' +
	    '</ViewFields>' +
    '</View>';
    query.set_viewXml(queryXML);
    this.listItems = list.getItems(query);
    ctx.load(this.web);
    ctx.load(this.list);
    ctx.load(this.listItems);
    ctx.executeQueryAsync(Function.createDelegate(this, this.onUISucceeded), Function.createDelegate(this, this.onQueryFailed));
}

function onUISucceeded(sender, args) {
    var likeHyperLink = document.getElementById(this.hlkLike);
    var script = likeHyperLink.getAttribute("onclick");
    if (this.flag) {
        likeHyperLink.innerText = "Unlike";
        likeHyperLink.setAttribute("onclick", script.replace("true", "false"));
    }
    else {
        likeHyperLink.innerText = "Like";
        likeHyperLink.setAttribute("onclick", script.replace("false", "true"));
    }
    console.log(this.listItems.length);
    var enumerator = this.listItems.getEnumerator();
    i = 0;
    while (enumerator.moveNext()) {
        i++;
    }
    document.getElementById(this.lblLikesCount).innerText = i;

}

function onQueryFailed(sender, args) {
    //alert('request failed ' + args.get_message() + '\n' + args.get_stackTrace());
}

function LikeUnlikeOperation(itemId, listId, webId, flag, hlkLike, lblLikesCount) {
    this.itemId = itemId;
    this.listId = listId;
    this.webId = webId;
    this.flag = flag;
    this.hlkLike = hlkLike;
    this.lblLikesCount = lblLikesCount;
    var ctx = new SP.ClientContext('/');
    this.web = ctx.get_web();
    this.currentUser = this.web.get_currentUser();
    this.list = this.web.get_lists().getByTitle("User Interest List");
    var query = new SP.CamlQuery();
    
    var queryXML = '<View>' +
	    '<Query>' +
		    '<Where>' +
                '<And>' +
				    '<And>' +
					    '<And>' +
						    '<Eq>' +
							    '<FieldRef Name="WebID"/><Value Type="Text">' + webId + '</Value>' +
						    '</Eq>' +
						    '<Eq>' +
							    '<FieldRef Name="ListID"/><Value Type="Text">' + listId + '</Value>' +
						    '</Eq>' +
					    '</And>' +
					    '<Eq>' +
						    '<FieldRef Name="ItemID"/><Value Type="Text">' + itemId + '</Value>' +
					    '</Eq>' +
				    '</And>' +
                    '<Eq>' +
                        '<FieldRef Name="User"/> <Value Type="Integer"><UserID/></Value>' +
                    '</Eq>' +
                '</And>' +
		    '</Where>' +
	    '</Query>' +
	    '<ViewFields>' +
		    '<FieldRef Name="User"/>' +
	    '</ViewFields>' +
	    '<RowLimit>1</RowLimit>' +
    '</View>';
    query.set_viewXml(queryXML);
    this.listItems = list.getItems(query);
    ctx.load(this.web);
    ctx.load(this.list);
    ctx.load(this.listItems);
    ctx.load(this.currentUser);
    ctx.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
}