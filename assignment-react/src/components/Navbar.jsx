import React from "react";
import { Link } from "react-router-dom";

// MUI Components
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';

// Icons
import { FaCartPlus } from "react-icons/fa";
import { FaHome } from "react-icons/fa";
import { FaSignInAlt } from "react-icons/fa";
import { FaUserPlus } from "react-icons/fa";
debugger
const Navbar = ({ isAuthenticated, role }) => {
  const userItems = [
    { label: "Home", path: "/home", icon: <FaHome style={{ marginRight: 8 }} /> },
    { label: "Top 10 New", path: "/books/top10news" },
    { label: "Borrowing", path: "/borrowing" },
  ];

  const guestItems = [
    { label: "Home", path: "/home", icon: <FaHome style={{ marginRight: 8 }} /> },
    { label: "Top 10 New", path: "/books/top10news" },
  ];

  const adminItems = [
    { label: "Book", path: "/admin/book" },
    { label: "Category", path: "/admin/category" },
    { label: "Borrowing", path: "/admin/borrowing" },
    { label: "Borrowing Extend", path: "/admin/borrowing-extend" },
  ];

  const renderMenuItems = (items) => (
    items.map((item, index) => (
      <Button
        key={index}
        color="inherit"
        component={Link}
        to={item.path}
        sx={{ 
          mx: 1,
          textTransform: 'none',
          fontSize: '1rem',
          fontWeight: 'normal',
          color: 'inherit',
          '&:hover': {
            backgroundColor: 'transparent',
            textDecoration: 'underline'
          }
        }}
        startIcon={item.icon || null}
      >
        {item.label}
      </Button>
    ))
  );

  return (
    <AppBar position="sticky" color="default" elevation={2}>
      <Toolbar sx={{ 
        display: 'flex', 
        justifyContent: 'space-between',
        padding: '0.5rem 2rem'
      }}>
        <Box display="flex" alignItems="center">
          <Link to="/" style={{ textDecoration: 'none', display: 'flex', alignItems: 'center' }}>
            <img src="/logo.png" alt="logo" style={{ width: 50, height: 50 }} />
            <Typography 
              variant="h6" 
              component="div" 
              sx={{ 
                ml: 2,
                fontWeight: 'bold',
                color: '#1976d2'
              }}
            >
              Library Management System
            </Typography>
          </Link>
        </Box>

        <Box display="flex" flexGrow={1} justifyContent="center">
          {role === "user"
            ? renderMenuItems(userItems)
            : role === "admin"
            ? renderMenuItems(adminItems)
            : renderMenuItems(guestItems)
          }
        </Box>

        <Box display="flex" alignItems="center" gap={2}>
          {isAuthenticated && role === "user" && (
            <IconButton color="inherit" component={Link} to="/cart">
              <FaCartPlus />
            </IconButton>
          )}
          
          {isAuthenticated ? (
            <Button 
              color="inherit"
              component={Link}
              to="/profile"
              sx={{
                textTransform: 'none',
                fontSize: '1rem'
              }}
            >
              Profile
            </Button>
          ) : (
            <>
              <Button 
                color="inherit" 
                component={Link} 
                to="/login"
                startIcon={<FaSignInAlt />}
                sx={{
                  textTransform: 'none',
                  fontSize: '1rem',
                  padding: '0.5rem 1.5rem',
                  borderRadius: '4px',
                  border: '1px solid #1976d2',
                  color: '#1976d2',
                  '&:hover': {
                    backgroundColor: 'rgba(25, 118, 210, 0.04)',
                    border: '1px solid #1976d2'
                  }
                }}
              >
                Login
              </Button>
              <Button 
                color="primary" 
                variant="contained" 
                component={Link} 
                to="/sign-up"
                startIcon={<FaUserPlus />}
                sx={{
                  textTransform: 'none',
                  fontSize: '1rem',
                  padding: '0.5rem 1.5rem',
                  borderRadius: '4px',
                  boxShadow: 'none',
                  '&:hover': {
                    backgroundColor: '#1565c0',
                    boxShadow: 'none'
                  }
                }}
              >
                Sign Up
              </Button>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;