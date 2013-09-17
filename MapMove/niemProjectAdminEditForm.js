//GLOBAL Var
niemApp = {
	isUSAddress : false,
	projectID : 0,
	map : undefined,
	addressQuery : [], //array of objects
	gotLatLong : false,
	viewFields : roboCAML.ViewFields([
		"WorkAddress", "WorkCity", "WorkState", "WorkZip", "WorkCountry", "IntlAddress"
	]),
	waitCounter : 0
};

$( document ).ready( function() {
	var $caseStudyId = $("input[title='CaseStudyId']"),
		$caseStudyLink = $("input[title='Case Study Link']")

	; //local vars

	$(".ms-standardheader > nobr").filter(function() {
		return $(this).text() === "Project ID";
	})
		.closest("tr");
//		.hide();

	$("select[title='Project ID']")
		.attr("disabled", "disabled");
		
	$("input[title='Title']")
		.closest("tr")
		.hide();

	$caseStudyLink.closest("span").closest("span").hide();

	$("input[title='CaseStudyId']")
		.closest("tr")
		.hide();


	if ( $.trim( $caseStudyId.val() ) !== "" && $caseStudyLink !== "" ) {
		var caseStudyLink = $caseStudyLink.val(),
			caseStudyFoundHtml = "<div id='case-study-links'><a href='" + caseStudyLink + "' target='_blank'><img alt='View Case Study' src='/Style Library/niem/js/planet/images/pdf_icon.png' style='padding-right: 2px; vertical-align: middle;' />View Case Study</a><br /><br />" +
				"<a href='javascript:void(0);' onclick='deleteCaseStudy();'><img alt='Delete Case Study' src='/Style Library/niem/js/planet/images/delete.png' style='padding-right: 1px; vertical-align: middle;' />Delete Case Study</a></div>"

		;//local vars

		//debugger;
		$caseStudyLink.closest("td").append( caseStudyFoundHtml );
	} else {
		var caseStudyAddHtml = "<div id='case-study-links'><a href='javascript:void(0);' onclick='openNewCaseStudyForm();'><img alt='Add New Case Study' src='/Style Library/niem/js/planet/images/add.png' style='padding-right: 2px; vertical-align: middle;' />Add New Case Study</a></div>"

		;
		$caseStudyLink.closest("td").append( caseStudyAddHtml );
	}

	niemApp.projectID = getValue( "Project ID" );

	var $save = $("input[value='Save']:last"),
		camlQuery = roboCAML.Query({
			listName: "NIEM Project Info",
			closeCaml: "SPServices",
			config: [
				{
					op: "=",
					staticName: "ID",
					value: niemApp.projectID
				}
			]
		})

	; //local vars


	//debugger;

	$().SPServices({
		operation: "GetListItems",
		listName: "NIEM Project Info",
		async: false,
		CAMLViewFields: niemApp.viewFields,
		CAMLQuery: camlQuery,
		completefunc: function( xData, Status ) {
			//console.log( Status );
			//console.log( xData.responseText );

			//debugger;

			$( xData.responseXML ).find("[nodeName='z:row']").each(function() {
				var $node = $(this)
				; //local vars

				if ( $node.attr( "ows_IntlAddress" ) == 1 ) {
					niemApp.isUSAddress = true;
					//var $latLongBtn = $save.clone();

					$("<input />", {
							id: "planet-get-LatLong",
							"class": "ms-ButtonHeightWidth",
							type: "button",
							name: "planet-get-LatLong",
							//onclick: "", //Removes default PreSaveAction
							value: "Get Lat/Long",
							click: getLatlong
					}).insertBefore( $save );

/*
					$latLongBtn
						.attr({
							id: "planet-get-LatLong",
							name: "planet-get-LatLong",
							onclick: "", //Removes default PreSaveAction
							value: "Get Lat/Long"
						})
						.insertBefore( $save )
						.click( getLatlong );
*/
				}
			});
		}
	});
});

//Util Funcs
function getValue( columnName ) {
	var ddlVal = $("select[title='" + columnName + "']").val();

	if ( ddlVal === undefined ) {
		ddlVal = $("input[title='" + columnName + "']").val();
	}

	return ddlVal;
}

function getQueryString() {
	var result = {}, queryString = location.search.substring(1),
	re = /([^&=]+)=([^&]*)/g,
	m;

	while ( m = re.exec( queryString ) ) {
		result[ decodeURIComponent( m[ 1 ] ) ] = decodeURIComponent( m[ 2 ] );
	}
	return result;
}
//End Util funcs

function PreSaveAction() {
debugger;
	var approveProject = $("input[title='Approve Project']").is(":checked"),
		latitude = $("input[title='Latitude']").val(),
		longitude = $("input[title='Longitude']").val(),
		bestOfNIEM = $("input[title='Best of NIEM']").is(":checked") ? 1 : 0,
		$caseStudy = $("input[title='Case Study Link']"),
		$caseStudyId = $("input[title='CaseStudyId']"),
		$title = $("input[title='Title']"),
		$approval = $("input[title='Approve Project']"),
		valuePairs = ['BestOfNIEM', bestOfNIEM, 'Longitude', longitude, 'Latitude', latitude, 'ID', niemApp.projectID]

	; //local vars


/*
	if ( $caseStudy.val() !== "http://" || $caseStudy.val() === "" ) {
		var caseStudyDesc = $.trim( $caseStudy.nextAll("input[title='Description']:first").val() ).length ?
			$caseStudy.nextAll("input[title='Description']:first").val() :
			$caseStudy.val(),
			caseStudyVal = $caseStudy.val() + ", " + caseStudyDesc,
			caseStudyId = $caseStudyId.val()

		; //local vars
		valuePairs.push( 'CaseStudy', "1" );
		valuePairs.push( 'CaseStudy1', caseStudyVal );
		valuePairs.push( 'CaseStudyId', caseStudyId );
	}
*/

	//debugger;

	var batchCMD = 	"<Batch OnError='Continue'>" +
										"<Method ID='1' Cmd='Update'>" +
											//Toggle this on to prevent workflow from firing and updating.
											"<Field Name='WebServiceUpdate'>1</Field>" +
											"<Field Name='BestOfNIEM'>" + bestOfNIEM + "</Field>" +
											"<Field Name='Longitude'>" + longitude + "</Field>" +
											"<Field Name='Latitude'>" + latitude + "</Field>" +
											"<Field Name='ID'>" + niemApp.projectID + "</Field>"
	;

	if ( $caseStudy.val() !== "http://" || $caseStudy.val() === "" ) {
		var caseStudyDesc = $.trim( $caseStudy.nextAll("input[title='Description']:first").val() ).length ?
			$caseStudy.nextAll("input[title='Description']:first").val() :
			$caseStudy.val(),
			caseStudyVal = $caseStudy.val() + ", " + caseStudyDesc,
			caseStudyId = $caseStudyId.val()

		; //local vars
									//Toggle on...
		batchCMD += 	"<Field Name='CaseStudy'>1</Field>" +
									//Hyperlink
									"<Field Name='CaseStudy1'>" + caseStudyVal + "</Field>" +
									//ID of file in Upload Case
									"<Field Name='CaseStudyId'>" + caseStudyId + "</Field>"
		;
	}

	batchCMD += "</Method>";


	if ( approveProject ) {
		$title.val("Project Approved");
		
		batchCMD +=	"<Method ID='2' Cmd='Moderate'>" +
										"<Field Name='ID'>" + niemApp.projectID + "</Field>" +
										"<Field Name='_ModerationStatus'>0</Field>" +
									"</Method>";
	}

	batchCMD += "</Batch>";



/*
	Currently roboCAML does not support Cmd='Moderate'   :'(
	Will be fixing that soon...
	roboCAML.BatchCMD({
			updates: [
					{
						valuePairs: valuePairs
					}
			]
		});
*/
	//Update Project Info with Lat/Long
	$().SPServices({
		operation: "UpdateListItems",
		listName: "NIEM Project Info",
		async: false,
		updates: batchCMD,
		completefunc: function( xData, Status ) {
			console.log( Status );
			console.log( xData.responseText );
			debugger;

		}
	});

/*
	//Update after the first update otherwise the item never gets approved.
	This was causing saving conflict errors, so I had to batch it altogether above.
	if ( approveProject ) {
		$title.val("Project Approved");
		var $caseStudyId = $("input[title='CaseStudyId']"),
			updateProjectStatus = "<Batch OnError='Continue'>" +
														"<Method ID='1' Cmd='Moderate'>" +
															"<Field Name='ID'>" + niemApp.projectID + "</Field>" +
															"<Field Name='_ModerationStatus'>0</Field>" +
														"</Method>" +
													"</Batch>"
		;

		$().SPServices({
			operation: "UpdateListItems",
			listName: "NIEM Project Info",
			async: false,
			updates: updateProjectStatus,
			completefunc: function( xData, Status ) {
				//console.log( Status );
				//console.log( xData.responseText );

				//debugger;
			}
		});
*/
	//Approve case study item if ID is found.
	if ( $caseStudyId.val() !== "" ) {
		var updateCaseStudyStatus = 	"<Batch OnError='Continue'>" +
																	"<Method ID='1' Cmd='Moderate'>" +
																		"<Field Name='ID'>" + $caseStudyId.val() + "</Field>" +
																		"<Field Name='_ModerationStatus'>0</Field>" +
																	"</Method>" +
																"</Batch>"
		;

		$().SPServices({
			operation: "UpdateListItems",
			listName: "UploadCase",
			async: false,
			updates: updateCaseStudyStatus,
			completefunc: function( xData, Status ) {
				//console.log( Status );
				//console.log( xData.responseText );

				//debugger;
			}
		});
	}
	return true;
}


function openCaseStudy( fileRef ) {
	OpenModalForm({
		staticListName: "UploadCase",
		url: fileRef,
		autoSize: true,
	});
}

function deleteCaseStudy() {
	var areYouSure = confirm("Are you sure you want to delete this case study?"),
		$caseStudyLink = $("input[title='Case Study Link']"),
		$caseStudyId = $("input[title='CaseStudyId']"),
		queryStrings = getQueryString(),
		batchCmd = "<Batch OnError='Continue'><Method ID='1' Cmd='Delete'><Field Name='ID'>" + $caseStudyId.val() + "</Field><Field Name='FileRef'>" + $caseStudyLink.val() + "</Field></Method></Batch>"
	; //local vars

	if ( !areYouSure ) return false;

	$().SPServices({
		operation: "UpdateListItems",
		listName: "UploadCase",
		async: false,
		updates: batchCmd,
		completefunc: function( xData, Status ) {
			//console.log( Status );
			//console.log( xData.responseText );
			var updateAdminList = "<Batch OnError='Continue'><Method ID='1' Cmd='Update'><Field Name='ID'>" + queryStrings.ID + "</Field><Field Name='CaseStudyId'></Field><Field Name='CaseStudy1'></Field></Method></Batch>",
				updateProjectList = 	"<Batch OnError='Continue'><Method ID='1' Cmd='Update'><Field Name='ID'>" + niemApp.projectID + "</Field><Field Name='CaseStudyId'></Field><Field Name='CaseStudy1'></Field><Field Name='CaseStudy'>0</Field><Field Name='WebServiceUpdate'>1</Field></Method></Batch>"
			; //local vars

			$().SPServices({
				operation: "UpdateListItems",
				listName: "NIEM Project Administration",
				async: false,
				updates: updateAdminList,
				completefunc: function( xData, Status ) {
					//console.log( Status );
					//console.log( xData.responseText );

				//	debugger;
					$().SPServices({
						operation: "UpdateListItems",
						listName: "NIEM Project Info",
						async: false,
						updates: updateProjectList,
						completefunc: function( xData, Status ) {
							//console.log( Status );
							//console.log( xData.responseText );

							//debugger;
							window.location = window.location.href;
						}
					});
				}
			});
			

			

/*
			var $links = $("#case-study-links");
			$links.html("");
			$caseStudyLink.val("");
			$caseStudyId.val("");
			$links.append("<a href='javascript:void(0);' onclick='openNewCaseStudyForm();'><img alt='Add New Case Study' src='/Style Library/niem/js/planet/images/add.png' style='padding-right: 2px; vertical-align: middle;' />Add New Case Study</a>");
*/
		}
	});
}

function openNewCaseStudyForm() {
	OpenModalForm({
		staticListName: "UploadCase",
		url: "/_layouts/Upload.aspx?List={FF6E4D3C-F3A8-4BD5-96AA-0003F6D8C5C3}&RootFolder=",
		autoSize: true,
		callback: function( dialogResult ) {
			var feedback = ( dialogResult ) ?
				"This item has been saved..." :
				"The modal window has been closed. Nothing has been modified..."
			;

			//alert( dialogResult );

			SP.UI.ModalDialog.commonModalDialogClose( dialogResult );
			SP.UI.Notify.addNotification( feedback, false );
			if ( dialogResult ) {
				//Id may be able to be passed as an args via modal call for edit form ~ New form not so much...
				var camlQuery = "<Query><Where><Eq><FieldRef Name='Author'/><Value Type='Integer'><UserID/></Value></Eq></Where><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query>";
				;

				$().SPServices({
					operation: "GetListItems",
					listName: "UploadCase",
					async: false,
					CAMLViewFields: "<ViewFields><FieldRef Name='FileRef' /><FieldRef Name='ID' /></ViewFields>",
					CAMLQuery: camlQuery,
					CAMLRowLimit: 1,
					completefunc: function( xData, Status ) {
						//console.log( Status );
						//console.log( xData.responseText );

						//debugger;

						$( xData.responseXML ).find("[nodeName='z:row']").each(function() {
							var $node = $(this),
								$caseStudyLink = $("input[title='Case Study Link']"),
								$caseStudyId = $("input[title='CaseStudyId']"),
								$links = $("#case-study-links"),
								fileRef =  "/" + $node.attr("ows_FileRef").split(";#")[1],
								caseStudyFoundHtml = "<a href='" + fileRef + "' target='_blank'><img alt='View Case Study' src='/Style Library/niem/js/planet/images/pdf_icon.png' style='padding-right: 2px; vertical-align: middle;' />View Case Study</a><br /><br />" +
									"<a href='javascript:void(0);' onclick='deleteCaseStudy();'><img alt='Delete Case Study' src='/Style Library/niem/js/planet/images/delete.png' style='padding-right: 1px; vertical-align: middle;' />Delete Case Study</a>"
							; //local vars

							$links.html("");
							$caseStudyLink.val( fileRef );
							$caseStudyLink.closest("td").find("input[title='Description']").val("Case Study");

							$caseStudyId.val( $node.attr("ows_ID") );

							$links.append( caseStudyFoundHtml );


						});
					}
				});
			}
		}
	});
}

function OpenModalForm( options ) {
	/**************************************************************************************************************
	// Why so many options M$? /smh
	// http://msdn.microsoft.com/en-us/library/ff410259
	//
	// Valid options listed here: //http://blogs.msdn.com/b/sharepointdev/archive/2011/01/13/using-the-dialog-platform.aspx
	*************************
	// These options are the bare minimum needed to open a modal dialog.
	// staticListName
	// ID
	*************************
	// formType ~ Default: DispForm
	// title
	// url
	// html
	// x ~ Default to center of axis
	// y ~ Default to center of axis
	// width: 800 ~ Default
	// height: 600 ~ Default.
	// allowMaximize: true ~ Default.
	// showMaximized: false ~ Default.
	// showClose: true ~ Default.
	// autoSize: false ~ Default.
	// callback: onDialogClose ~ Default.

	********************************************************************
	Use args to pass data to the modal.  Access using: window.frameElement.dialogArgs
	*********************************************************************
	// args: {} ~ Default.
	***************************************************************************************************************/

	// Safeguard the options param
	options = options || {};
	//options.formType = options.formType || "display";
	//url will find current site for each scenario
	var url,
		formType
	; //local vars
	//L_Menu_BaseUrl --- //http://community.zevenseas.com/Blogs/Vardhaman/Lists/Posts/Post.aspx?ID=9

	if ( options.hasOwnProperty("url") ) {
		//Locates full path URL's or relative URL's
		if ( options.url.substring( 0,1 ) === "." || options.url.substring( 0,4 ) === "http" ) {
			url = options.url;
		} else {
			url = L_Menu_BaseUrl + options.url;
		}
		//deletes property to prevent overwriting when extending options
		delete options.url;
	} else {
		try {
			switch ( options.formType.toLowerCase() ) {
				case "display":
					formType = "DispForm";
					break;
				case "edit":
					formType = "EditForm";
					break;
				case "new":
					formType = "NewForm";
					break;
				default:
					formType = "DispForm";
					break;
			}
		} catch( e ) {
			formType = "CustomDispForm";
		}

		//Default the base URL to the url variable
		if ( L_Menu_BaseUrl === "" ) {
			url = "/Lists/" + options.staticListName + "/" + formType + ".aspx?ID=" + options.ID;
		} else {
			url = L_Menu_BaseUrl + "/Lists/" + options.staticListName + "/" + formType + ".aspx?ID=" + options.ID;
		}
	}

	//Rid jQuery dependency on this method...
	var opt = {
		title: options.title || "",
		url: url,
		html: options.html || undefined,
		height: options.height || 600,
		width: options.width || 800,
		allowMaximize: options.allowMaximize || true,
		showMaximized: options.showMaximized || false,
		showClose: options.showClose || true,
		autoSize: options.autoSize || false,
		dialogReturnValueCallback: options.callback || function() {},
		//Use args to pass data to the modal.  Access using: window.frameElement.dialogArgs
		args: options.args || {}
	};

	//debugger;
	//Create modal
	ExecuteOrDelayUntilScriptLoaded(
		function() {
			SP.UI.ModalDialog.showModalDialog( opt );
		},
		'sp.js'
	);
}

/**************************
// Map Funcs
**************************/
function getLatlong( e ) {
	e.preventDefault();
	$("#planet-get-LatLong")[0].disabled = true;

	var item = {},
		camlQuery = roboCAML.Query({
			listName: "NIEM Project Info",
			closeCaml: "SPServices",
			config: [
				{
					op: "=",
					staticName: "ID",
					value: niemApp.projectID
				}
			]
		}),
		validationError = "<br /><SPAN class=ms-formvalidation><SPAN role=alert>You must specify a value for this required field.</SPAN><BR></SPAN>"

	; //local vars

	//add spinner to getLatLong input
	$("<img id='planet-pleaseWait' src='../../Style Library/niem/js/planet/images/ajax-loader-small.gif' />")
		.insertBefore("#planet-get-LatLong");

	$().SPServices({
		operation: "GetListItems",
		listName: "NIEM Project Info",
		async: false,
		CAMLViewFields: niemApp.viewFields,
		CAMLQuery: camlQuery,
		completefunc: function( xData, Status ) {
			//console.log( Status );
			//console.log( xData.responseText );

			$( xData.responseXML ).find("[nodeName='z:row']").each(function() {
				var $node = $(this)
				; //local vars

				item.address = $node.attr("ows_WorkAddress");
				item.city = $node.attr("ows_WorkCity");
				item.state = $node.attr("ows_WorkState");
				item.zip = $node.attr("ows_WorkZip");
				item.country = $node.attr("ows_WorkCountry");
			});

			//Used when querying Bing
			item.bingAddress = item.address + " " + item.city + " " + item.state + " " + item.zip;
			//cache properties in global for easy arg passing.
			niemApp.addressQuery.push( item );
			//Query Bing for lat/long
			updateLocation();
		}
	});
}

function updateLocation() {
	//debugger;
	//console.log( "updateLocation: " + this.id + "\nAddress: " + this.bingAddress );
	niemApp.map = new Microsoft.Maps.Map(
		document.getElementById('pl-bing-map'),
		{
			credentials: "AnnArQwNIgUpJT2pwG-r8eNlG9bD1s80u6l4vk-Ar24jRnh_bQO25Mv_5gSb8t3v"
		}
	);
	niemApp.map.getCredentials( callSearchService );
	//console.log( "Final piece of code in updateLocation: " + niemApp.gotLatLong );
}

function callSearchService( credentials ) {
	//May have to do an unstructured URL
	//http://msdn.microsoft.com/en-us/library/ff701711

	//debugger;

	var lookupItem = niemApp.addressQuery[ niemApp.addressQuery.length - 1 ],
		searchRequest = 'http://dev.virtualearth.net/REST/v1/Locations/' + lookupItem.bingAddress + '?output=json&jsonp=searchServiceCallback&key=' + credentials,
		mapscript = document.createElement('script')
	; //local vars


	mapscript.type = 'text/javascript';
	mapscript.src = searchRequest;
	document.getElementById("pl-bing-map").appendChild( mapscript );
	//console.log( "Final piece of code in callSearchService: " + niemApp.gotLatLong );
}

function searchServiceCallback( result ) {
	var $latitude = $("input[title='Latitude']"),
		$longitude = $("input[title='Longitude']")
	; //local vars

	niemApp.gotLatLong = true;
	//console.log( "After setting to true in searchServiceCallback: " + niemApp.gotLatLong );
	if (
		result &&
		result.resourceSets &&
		result.resourceSets.length > 0 &&
		result.resourceSets[0].resources &&
		result.resourceSets[0].resources.length > 0
	) {


	//debugger;


		var coords = result.resourceSets[0].resources[0].point.coordinates

		; //local vars

		$latitude.val( coords[0] );
		$longitude.val( coords[1] );
	}
	else {
		if ( typeof ( response ) == 'undefined' || response == null ) {
			$latitude.val("PLEASE UPDATE THIS. ERROR: Invalid credentials or no response");
			$longitude.val("PLEASE UPDATE THIS. ERROR: Invalid credentials or no response");
		} else {
			$latitude.val("PLEASE UPDATE THIS. ERROR: No results for the query");
			$longitude.val("PLEASE UPDATE THIS. ERROR: No results for the query");
		}
	}

	$("#planet-get-LatLong")[0].disabled = false;
	$("#planet-pleaseWait").remove();

	//debugger;
}