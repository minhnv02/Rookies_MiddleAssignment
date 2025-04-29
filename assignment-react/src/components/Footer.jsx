import React from "react";
import { Box, Container, Typography, Link, Divider, IconButton } from "@mui/material";
import { Facebook, Twitter, Instagram, LinkedIn, GitHub } from "@mui/icons-material";

const Footer = () => {
  const currentYear = new Date().getFullYear();

  return (
    <Box
      component="footer"
      sx={{
        backgroundColor: (theme) => theme.palette.grey[100],
        py: 6,
        mt: "auto",
        fontSize: "0.9rem",
      }}
    >
      <Container maxWidth="lg">
        {/* Top sections */}
        <Box
          sx={{
            display: "flex",
            flexDirection: { xs: "column", md: "row" },
            justifyContent: "space-evenly",
            textAlign: { xs: "center", md: "left" },
            gap: { xs: 5, md: 10 },
            mb: 4,
          }}
        >
          {/* About */}
          <Box sx={{ maxWidth: 300 }}>
            <Typography variant="h6" color="primary" gutterBottom>
              Library Management System
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Your complete solution for managing library resources, borrowing books, and discovering new reading materials.
            </Typography>
          </Box>

          {/* Quick Links */}
          <Box sx={{ maxWidth: 150 }}>
            <Typography variant="h6" color="primary" gutterBottom>
              Quick Links
            </Typography>
            <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
              {["Home", "About Us", "Services", "Contact"].map((item, index) => (
                <Link 
                  key={index}
                  href="#"
                  color="text.secondary"
                  underline="hover"
                  sx={{
                    fontSize: "0.9rem",
                    "&:hover": { color: "primary.main" },
                  }}
                >
                  {item}
                </Link>
              ))}
            </Box>
          </Box>

          {/* Contact */}
          <Box sx={{ maxWidth: 250 }}>
            <Typography variant="h6" color="primary" gutterBottom>
              Contact Us
            </Typography>
            <Typography variant="body2" color="text.secondary">
              123 Library Street, Book City
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Email: info@library.com
            </Typography>
            <Typography variant="body2" color="text.secondary" gutterBottom>
              Phone: (123) 456-7890
            </Typography>

            <Box sx={{ display: "flex", justifyContent: { xs: "center", md: "flex-start" }, gap: 1, mt: 1 }}>
              {[Facebook, Twitter, Instagram, LinkedIn, GitHub].map((Icon, idx) => (
                <IconButton key={idx} color="primary" size="small" sx={{ p: 0.5 }}>
                  <Icon fontSize="small" />
                </IconButton>
              ))}
            </Box>
          </Box>
        </Box>

        <Divider sx={{ my: 4 }} />

        {/* Bottom */}
        <Box
          sx={{
            display: "flex",
            flexDirection: { xs: "column", md: "row" },
            justifyContent: "space-between",
            alignItems: "center",
            textAlign: { xs: "center", md: "left" },
            gap: 2,
          }}
        >
          <Typography variant="body2" color="text.secondary">
            © {currentYear} Library Management System. All rights reserved.
          </Typography>

          <Box sx={{ display: "flex", gap: 2 }}>
            <Link href="#" color="text.secondary" underline="hover" sx={{ "&:hover": { color: "primary.main" } }}>
              Privacy Policy
            </Link>
            <Link href="#" color="text.secondary" underline="hover" sx={{ "&:hover": { color: "primary.main" } }}>
              Terms of Service
            </Link>
          </Box>
        </Box>
      </Container>
    </Box>
  );
};

export default Footer;
