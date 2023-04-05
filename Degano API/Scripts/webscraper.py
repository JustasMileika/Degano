from bs4 import BeautifulSoup
import requests
import firebase_admin
from firebase_admin import credentials
from firebase_admin import db
from datetime import datetime
import os



def get_fuel_data():

    os.chdir(os.path.dirname(os.path.abspath(__file__))) # sets current directory to where the file is located

    print(os.getcwd())

    # Initializing the database
    cred = credentials.Certificate(
        "degano-70426-firebase-adminsdk-q34pc-1ba5178613.json")
    firebase_admin.initialize_app(
        cred, {'databaseURL': 'https://degano-70426-default-rtdb.europe-west1.firebasedatabase.app/'})
    ref = db.reference()
    ref.child("Updated").update(
        {"Datetime": datetime.now().strftime("%Y %m %d %H %M %S")})
    ref = db.reference("Degano/")
    gas_stations = ref.get()

    # Initializing BS for the specified website
    html_text = requests.get(
        'https://gas.didnt.work/?country=lt&brand=&city=Vilnius').text
    soup = BeautifulSoup(html_text, 'lxml')

    # Scraping information from the specified site
    tbody = soup.find('tbody')
    trs = tbody.findAll('tr')
    for tr in trs:
        tr.find('span').decompose()
        tr.find('br').decompose()
        address = tr.find('small').text
        modified_address = address.replace(
            ", Vilnius", "").replace(".", "*").replace("/", "@")
        tr.find('small').decompose()
        gas_station_name = tr.find('td').text[2:]
        diesel = tr.find('td').findNext('td').text
        petrol95 = tr.find('td').findNext('td').findNext('td').text
        petrol98 = tr.find('td').findNext(
            'td').findNext('td').findNext('td').text
        LPG = tr.find('td').findNext('td').findNext(
            'td').findNext('td').findNext('td').text

        # Updating all of the prices for the gas stations in the Firebase Realtime database
        if diesel and petrol95 != "-" and gas_station_name.strip() != "A. Lingės degalinė":
            for key, value in gas_stations.items():
                if key == modified_address:
                    ref.child(key).update({"Diesel": str(diesel)})
                    ref.child(key).update({"Petrol95": str(petrol95)})
                    ref.child(key).update({"Petrol98": str(petrol98)})
                    ref.child(key).update({"LPG": str(LPG)})


if __name__ == '__main__':
    get_fuel_data()
