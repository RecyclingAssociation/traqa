# Traqa

Led by industry trade body The Recycling Association, Traqa is a collaborative platform to automate the transfer of information between all parties in the recycling supply chain, tracking loads for export from origin to end destination using the latest in cutting edge technology.

The Recycling Association have created some data structures to facilitate the transfer of this information and create a standardised dataset that meets the requirements of the Pickup to Port process.

There are 2 key points that data needs to be transferred to create efficiency during this process.

1. Load Plan
2. Weighbridge Data

# Load Plan 

The shipper needs to provide to the load site enough information to pre-populate their software to remove unecessary manual entry and provide an expected load plan to thier system.

It is key that the loading references are unique to the shipper, as when the container is loaded the weighing information can be matched and returned to the shipper automatically.

The load sites require:

* Shipper
* PO Number
* Description of Goods 
* Loading Reference
* Planned Loading DateTime
* Port of Loading 
* Port of Discharge
* Carrier

# Weighbridge Data 

The weighbridge software will need to capture from the loading site the following information to complete the dataset and enable the container to move efficiently through the supply chain.

* Container Number
* Seal Number
* No Bales
* Net Weight / Product Weight 
* Verified Gross Mass (VGM Weight)
* Haulier 
* Vehicle Registration

# Codes and Identifiers

The correct identification of a party in the supply chain is key for compliance and also the correct allocation of data, to facilitate this we promote the use of identity that is in use for the domain and the region or from an international code list.

## EORI Number to Identify the Shipper or Exporter

An EORI number – which stands for an Economic Operator Registration and Identification Number – is a unique ID code used to track and register customs information.

Traqa use this to identify the **Exporter** of the material, in the absence of this you will use a Traqa assigned id prefixed with TQ.

UK EORI must start with 'GB', https://www.gov.uk/check-eori-number

EU EORI Checker https://ec.europa.eu/taxation_customs/dds2/eos/eori_validation.jsp?Lang=en

## Load Sites will use the BIC Facility Code

Any facility which can accept containers for purposes of loading, unloading, repair or storage are eligable to use the Bureau International des Container (BIC) Facility Code, further details can be found on this global code list at https://www.bic-code.org/locodes/

Each **Load Site** will be assigned a BIC Facility code which will be used to identify where the loads will take place.

## Carrier - Haulier or Shipping Line 

The Haulier's will be identified using the **Waste Carrier License** as this is known to all parties and mandatory for the movement of green list waste.  These codes will be validated on receipt.

The Shipping Line when moving by ocean will use the carrier **SCAC code** and this is available under the 'codes' sub folder.




