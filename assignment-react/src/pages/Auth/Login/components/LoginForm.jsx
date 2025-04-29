import React, { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { AuthContext } from "../../../../contexts/AuthContext";
import { message } from "antd";

// MUI Components
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import Link from '@mui/material/Link';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { styled } from '@mui/material/styles';

const StyledPaper = styled(Paper)(({ theme }) => ({
  padding: theme.spacing(4),
  maxWidth: 400,
  margin: 'auto',
  marginTop: theme.spacing(2),
  boxShadow: theme.shadows[3],
  borderRadius: '8px',
}));

const LoginForm = () => {
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await axiosInstance.post("/auths/login", values);
      login(response.data.data);
      if (response.data.data.role === "admin") {
        navigate("/admin");
      } else navigate("/home");
    } catch (error) {
      if (error.response && error.response.status === 409) {
        message.error(error.response.data);
      } else {
        message.error("Login failed");
      }
    }
    setLoading(false);
  };

  return (
    <Container component="main" maxWidth="xs">
      <Typography 
        component="h1" 
        variant="h4" 
        align="center" 
        sx={{ 
          fontWeight: 'bold', 
          mb: 2,
          color: '#1976d2',
          marginTop: 4
        }}
      >
        Library Management System
      </Typography>
      
      <StyledPaper elevation={3}>
        <Typography 
          variant="h5" 
          align="center" 
          gutterBottom
          sx={{ 
            mb: 2,
            fontWeight: 'bold',
            backgroundColor: '#f5f5f5',
            padding: '8px',
            borderRadius: '4px',
            color: '#333'
          }}
        >
          Login
        </Typography>
        
        <Typography 
          variant="body1" 
          align="center" 
          color="text.secondary"
          sx={{ mb: 4 }}
        >
          Sign in to manage the book lending system
        </Typography>
        
        <Box 
          component="form" 
          onSubmit={(e) => {
            e.preventDefault();
            const formData = new FormData(e.currentTarget);
            onFinish({
              email: formData.get('email'),
              password: formData.get('password')
            });
          }}
        >
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email *"
            name="email"
            autoComplete="email"
            autoFocus
            variant="outlined"
            sx={{ mb: 2 }}
          />
          
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password *"
            type={showPassword ? 'text' : 'password'}
            id="password"
            autoComplete="current-password"
            variant="outlined"
            sx={{ mb: 2 }}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle password visibility"
                    onClick={handleClickShowPassword}
                    onMouseDown={handleMouseDownPassword}
                    edge="end"
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />
          
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ 
              mt: 3, 
              mb: 2, 
              py: 1.5,
              backgroundColor: '#1976d2',
              fontSize: '1rem',
              textTransform: 'none',
              '&:hover': {
                backgroundColor: '#1565c0',
              }
            }}
            disabled={loading}
          >
            {loading ? (
              <CircularProgress size={24} color="inherit" />
            ) : (
              'Login'
            )}
          </Button>
          
          <Typography variant="body2" align="center" sx={{ mt: 2 }}>
            Don't have an account?{' '}
            <Link href="/sign-up" color="primary" underline="hover">
              Signup now
            </Link>
          </Typography>
        </Box>
      </StyledPaper>
    </Container>
  );
};

export default LoginForm;