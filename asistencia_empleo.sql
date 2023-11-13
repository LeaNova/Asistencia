-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 13-11-2023 a las 20:22:31
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `asistencia_empleo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `asistencias`
--

CREATE TABLE `asistencias` (
  `codIngreso` varchar(10) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `horaIngreso` varchar(10) DEFAULT NULL,
  `horaSalida` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `asistencias`
--

INSERT INTO `asistencias` (`codIngreso`, `idUsuario`, `horaIngreso`, `horaSalida`) VALUES
('09112023G6', 2, NULL, NULL),
('09112023G6', 3, NULL, NULL),
('09112023G6', 4, NULL, NULL),
('12112023Zd', 2, '10:22 PM', '11:01 PM'),
('12112023Zd', 3, NULL, NULL),
('12112023Zd', 4, NULL, NULL),
('12112023Zd', 9, NULL, NULL),
('12112023Zd', 10, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estciviles`
--

CREATE TABLE `estciviles` (
  `idEstCivil` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `estciviles`
--

INSERT INTO `estciviles` (`idEstCivil`, `nombre`) VALUES
(1, 'Soltero/a'),
(2, 'Casado/a'),
(3, 'Divorciado/a'),
(4, 'Biudo/a');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `familiares`
--

CREATE TABLE `familiares` (
  `idFamilia` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(10) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `idRelacion` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `generos`
--

CREATE TABLE `generos` (
  `idGenero` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `generos`
--

INSERT INTO `generos` (`idGenero`, `nombre`) VALUES
(1, 'Masculino'),
(2, 'Femenino'),
(3, 'No binario'),
(4, 'Otros');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ingresos`
--

CREATE TABLE `ingresos` (
  `codIngreso` varchar(10) NOT NULL,
  `fecha` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ingresos`
--

INSERT INTO `ingresos` (`codIngreso`, `fecha`) VALUES
('09112023G6', '2023-11-09'),
('12112023Zd', '2023-11-12'),
('27102023rD', '2023-10-27'),
('31102023DM', '2023-10-31');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `relaciones`
--

CREATE TABLE `relaciones` (
  `idRelacion` int(11) NOT NULL,
  `nombre` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `idRol` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  `disponible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`idRol`, `nombre`, `disponible`) VALUES
(1, 'Admin', 0),
(2, 'Jefe', 1),
(3, 'Empleado', 1),
(4, 'Guardia', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `idUsuario` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `fechaNac` date NOT NULL,
  `dni` varchar(9) NOT NULL,
  `idGenero` int(11) NOT NULL,
  `idEstCivil` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `mail` varchar(100) NOT NULL,
  `pass` varchar(60) NOT NULL,
  `fechaIngreso` date NOT NULL,
  `idRol` int(11) NOT NULL,
  `disponible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`idUsuario`, `nombre`, `apellido`, `fechaNac`, `dni`, `idGenero`, `idEstCivil`, `direccion`, `telefono`, `mail`, `pass`, `fechaIngreso`, `idRol`, `disponible`) VALUES
(1, 'Admin', 'Admin', '2023-10-17', '000000000', 1, 1, 'Empresa', '0000000000', 'admin@admin.com', 'E+yozJZ8qCSQL0cOMij0qs0KAVKj3HZQi39lyNu4nLI=', '2023-10-17', 1, 1),
(2, 'Leandro', 'Heredia', '1996-08-06', '3961', 1, 1, 'si', '266489', 'lea@hotmail.com', '1FL0BX8yOInH3RPvkEKM4LNPu4MBVDzTJuTQFgFjvME=', '2023-10-23', 3, 1),
(3, 'Luis', 'Ramirez', '1991-03-21', '27444220', 1, 2, 'Alguna casa', '26641105298', 'luis@mail.com', 'E+yozJZ8qCSQL0cOMij0qs0KAVKj3HZQi39lyNu4nLI=', '2023-10-23', 3, 1),
(4, 'Amanda', 'Tomayo', '2002-12-12', '53759523', 2, 1, 'La direccion', '2665258790', 'amanda@mail.com', 'E+yozJZ8qCSQL0cOMij0qs0KAVKj3HZQi39lyNu4nLI=', '2023-10-26', 3, 1),
(9, 'Alejandra', 'Muñoz', '2023-11-01', '12333', 1, 2, 'Entre las calles 150', '1557895', 'ale@hotmail.com', 'j7kX2gbgMD6OqFOabPwkuvaeLWE2yqqvi77NUJaYHS8=', '2023-11-09', 2, 1),
(10, 'Rita', 'Hill', '1995-02-10', '5789998', 2, 2, 'Calle falsa 23', '15889', 'hill@hotmail.com', 'TEG1pn9l0wdmv9isgKuZ3oaZEMiZfrSB/CzpJk3M5fk=', '2023-11-11', 4, 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `asistencias`
--
ALTER TABLE `asistencias`
  ADD PRIMARY KEY (`codIngreso`,`idUsuario`),
  ADD KEY `id_usuario_asis` (`idUsuario`);

--
-- Indices de la tabla `estciviles`
--
ALTER TABLE `estciviles`
  ADD PRIMARY KEY (`idEstCivil`);

--
-- Indices de la tabla `familiares`
--
ALTER TABLE `familiares`
  ADD PRIMARY KEY (`idFamilia`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD KEY `id_rela_fam` (`idRelacion`),
  ADD KEY `id_rela_usu` (`idUsuario`);

--
-- Indices de la tabla `generos`
--
ALTER TABLE `generos`
  ADD PRIMARY KEY (`idGenero`);

--
-- Indices de la tabla `ingresos`
--
ALTER TABLE `ingresos`
  ADD PRIMARY KEY (`codIngreso`);

--
-- Indices de la tabla `relaciones`
--
ALTER TABLE `relaciones`
  ADD PRIMARY KEY (`idRelacion`);

--
-- Indices de la tabla `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`idRol`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`idUsuario`),
  ADD UNIQUE KEY `dni_usuario` (`dni`),
  ADD UNIQUE KEY `mail_usuario` (`mail`),
  ADD UNIQUE KEY `tel_usuario` (`telefono`),
  ADD KEY `usuarioRol` (`idRol`),
  ADD KEY `usuarioGenero` (`idGenero`),
  ADD KEY `usuarioCivil` (`idEstCivil`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `estciviles`
--
ALTER TABLE `estciviles`
  MODIFY `idEstCivil` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `familiares`
--
ALTER TABLE `familiares`
  MODIFY `idFamilia` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `generos`
--
ALTER TABLE `generos`
  MODIFY `idGenero` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `relaciones`
--
ALTER TABLE `relaciones`
  MODIFY `idRelacion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `roles`
--
ALTER TABLE `roles`
  MODIFY `idRol` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `asistencias`
--
ALTER TABLE `asistencias`
  ADD CONSTRAINT `cod_ingreso_asis` FOREIGN KEY (`codIngreso`) REFERENCES `ingresos` (`codIngreso`) ON DELETE CASCADE,
  ADD CONSTRAINT `id_usuario_asis` FOREIGN KEY (`idUsuario`) REFERENCES `usuarios` (`idUsuario`);

--
-- Filtros para la tabla `familiares`
--
ALTER TABLE `familiares`
  ADD CONSTRAINT `id_rela_fam` FOREIGN KEY (`idRelacion`) REFERENCES `relaciones` (`idRelacion`),
  ADD CONSTRAINT `id_rela_usu` FOREIGN KEY (`idUsuario`) REFERENCES `usuarios` (`idUsuario`);

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `usuarioCivil` FOREIGN KEY (`idEstCivil`) REFERENCES `estciviles` (`idEstCivil`),
  ADD CONSTRAINT `usuarioGenero` FOREIGN KEY (`idGenero`) REFERENCES `generos` (`idGenero`),
  ADD CONSTRAINT `usuarioRol` FOREIGN KEY (`idRol`) REFERENCES `roles` (`idRol`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
