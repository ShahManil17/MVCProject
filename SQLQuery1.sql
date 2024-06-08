CREATE TABLE role_has_permissions (
	role_id INT,
	permission_id INT,
	FOREIGN KEY (role_id) REFERENCES roles(id),
	FOREIGN KEY (permission_id) REFERENCES permissions(id)
);

INSERT INTO role_has_permissions(role_id, permission_id) VALUES (1, 1), (1, 2), (1, 3), (1, 4), (1, 8), (1, 10);
INSERT INTO role_has_permissions(role_id, permission_id) VALUES (2, 1), (2, 2), (2, 4), (2, 8), (2, 10);
INSERT INTO role_has_permissions(role_id, permission_id) VALUES (3, 8), (3, 10);

DELETE FROM user_has_permissions WHERE permission_id <> 7;

SELECT perm.name
FROM permissions AS perm
INNER JOIN role_has_permissions AS rhp ON perm.id = rhp.permission_id
INNER JOIN roles AS role ON rhp.role_id = role.id
WHERE role.name = 'manager';

SELECT name FROM permissions WHERE id IN (SELECT permission_id FROM user_has_permissions WHERE user_id = 1);
SELECT name FROM permissions WHERE id IN (SELECT permission_id FROM role_has_permissions WHERE role_id = (SELECT role_id FROM users WHERE id = 1));


SELECT * FROM permissions;
SELECT * FROM role_has_permissions;
SELECT * FROM user_has_permissions ORDER BY user_id;