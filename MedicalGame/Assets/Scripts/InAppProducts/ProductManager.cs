using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using System;
using LitJson_Gamedonia;
using UnityEngine.UI;


[Prefab("ProductManager", false, "")]
public class ProductManager : Singleton<ProductManager> {

	//public string[] products_list = new string[]{"android.test.purchased"};
	public string[] products_list = new string[]{"m_lives"};
	public Text debugText;
	

	// Use this for initialization
	void Start () {
		// In App purchases, request callback
		GDInAppService reqService = new GDInAppService();
		reqService.RegisterEvent += new InAppEventHandler(OnProductsRequested);
		GamedoniaStoreInAppPurchases.AddRequestService(reqService);
		// In App purchases, buyservice callback
		GDInAppService buyService = new GDInAppService();
		buyService.RegisterEvent += new InAppEventHandler(OnProductPurchased);
		GamedoniaStoreInAppPurchases.AddPurchaseService(buyService);
	
	}
	
	public bool Load() {return true;}
	
	public void BuyProduct() {
		//GamedoniaStore.RequestProducts(consumable_products, consumable_products.Length);
		GamedoniaStore.BuyProduct("m_lives");
	}
	
	public void RequestProducts() {
		GamedoniaStore.RequestProducts(products_list, products_list.Length);
	}
	
	private void OnProductsRequested() {
		
		debugText.text+= "before succes - ";
		if (GamedoniaStore.productsRequestResponse.success) {
	 
			debugText.text+= "succesfully requested products - ";
			foreach (KeyValuePair<string, Product> entry in GamedoniaStore.productsRequestResponse.products) {
	 
				Product product = (Product)entry.Value;
				debugText.text+= " [Received Product: " + product.identifier + " price: " + product.priceLocale + " description: " + product.description+"] ";
			}
		} else {
			debugText.text+= "Unable to request products!, message: " + GamedoniaStore.productsRequestResponse.message;
		}
	}
	
	private void OnProductPurchased() {
		debugText.text+= " - Product purchased -";
		PurchaseResponse purchase = GamedoniaStore.purchaseResponse;
		string details = "Purchase Result status: " + purchase.status + " for product identifier: " + purchase.identifier;
		if (purchase.message != null && purchase.message.Length > 0) details += " message: " + purchase.message;
		 
		Debug.Log(details);
		debugText.text+= details;
		if (purchase.success) {
	
			debugText.text+= " - Purchase success";
			// UNLOCK YOUR NEW MAP
		} else {
			debugText.text+= " - Something went wrong";
		}
	}

}
