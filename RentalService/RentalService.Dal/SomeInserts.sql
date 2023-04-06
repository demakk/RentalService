Use RentalServiceApp

INSERT INTO Countries (Name)
VALUES 
('Ukraine'),
('Poland'),
('China')


INSERT INTO Cities (CountryId, Name)
VALUES
(1, 'Kyiv'),
(1, 'Kharkiv'),
(1, 'Odesa'),
(2, 'Wroclaw'),
(2, 'Krakow'),
(2, 'Warsaw'),
(3, 'Bejin'),
(3, 'Shanghai'),
(3, 'Chongqing ')

INSERT INTO Manufacturers(Id, CountryId, Name, Description)
VALUES
('7e05b587-1348-4c0a-b7c6-c4b299bad58c', 1, 'TestManufacturer', 'SomeDescription')

INSERT INTO ItemCategories(Id, Name)
VALUES 
('62c1c16d-9bb4-43d6-8885-23bf6a8a4bb9', 'Trekking poles')

INSERT INTO Items(Id, ItemCategoryId, ManufacturerId, InitialPrice, CurrentPrice, Description, ItemStatus)
VALUES
('0fcc4fb8-79f2-4a10-af4b-b41c36403252', '62c1c16d-9bb4-43d6-8885-23bf6a8a4bb9', '7e05b587-1348-4c0a-b7c6-c4b299bad58c',
123, 123, 'SomeItemDescription', 1)