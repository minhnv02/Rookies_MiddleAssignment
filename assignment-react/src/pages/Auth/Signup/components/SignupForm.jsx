import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
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

const SignupForm = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleClickShowConfirmPassword = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await axiosInstance.post("/auths/register", values);
      const email = response.data.data.email;
      await axiosInstance.get(`/auths/request-active-account/${email}`);
      message.success(response.data.message);
      navigate("/login");
    } catch (error) {
      if (error.response && error.response.status === 409) {
        message.error(error.response.data);
      } else {
        message.error("Registration failed");
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
          Sign Up
        </Typography>
        
        <Box 
          component="form" 
          onSubmit={(e) => {
            e.preventDefault();
            const formData = new FormData(e.currentTarget);
            onFinish({
              email: formData.get('email'),
              name: formData.get('name'),
              password: formData.get('password'),
              confirmPassword: formData.get('confirmPassword'),
              roleId: 2
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
            id="name"
            label="Name *"
            name="name"
            autoComplete="name"
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
            autoComplete="new-password"
            variant="outlined"
            sx={{ mb: 2 }}
            helperText="Password must be at least 8 characters with uppercase, lowercase, number and special character"
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
          
          <TextField
            margin="normal"
            required
            fullWidth
            name="confirmPassword"
            label="Confirm Password *"
            type={showConfirmPassword ? 'text' : 'password'}
            id="confirmPassword"
            autoComplete="new-password"
            variant="outlined"
            sx={{ mb: 2 }}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle confirm password visibility"
                    onClick={handleClickShowConfirmPassword}
                    onMouseDown={handleMouseDownPassword}
                    edge="end"
                  >
                    {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />
          
          <input type="hidden" name="roleId" value={2} />
          
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
              'Sign Up'
            )}
          </Button>
          
          <Typography variant="body2" align="center" sx={{ mt: 2 }}>
            Already have an account?{' '}
            <Link href="/login" color="primary" underline="hover">
              Login now
            </Link>
          </Typography>
        </Box>
      </StyledPaper>
    </Container>
  );
};

export default SignupForm;